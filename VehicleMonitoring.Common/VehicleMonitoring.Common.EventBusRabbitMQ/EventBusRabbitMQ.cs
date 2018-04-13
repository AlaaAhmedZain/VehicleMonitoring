using Autofac;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using VehicleMonitoring.Common.EventBus.Events;
using VehicleMonitoring.Common.EventBus.Interfaces;
using VehicleMonitoring.Common.EventBus.Subscriptions;

namespace VehicleMonitoring.Common.EventBusRabbitMQ
{
    public class RabbitMQBus : IEventBus, IDisposable
    {
        private readonly string BROKER_NAME = "VehicleStatusMonitoringBus";
        private readonly string AUTOFAC_SCOPE_NAME = "VehicleStatusBus";

        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly ILogger<RabbitMQBus> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly ILifetimeScope _autofac;
        private readonly int _retryCount;
        private IModel _consumerChannel;
        private string _queueName;

        public RabbitMQBus(IRabbitMQPersistentConnection persistentConnection, ILogger<RabbitMQBus> logger,
            ILifetimeScope autofac, IEventBusSubscriptionsManager subsManager, string queueName = null, int retryCount = 5)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new InMemoryEventBusSubscriptionsManager();
            _queueName = queueName;
            _consumerChannel = CreateConsumerChannel();
            _autofac = autofac;
            _retryCount = retryCount;
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved;
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            using (var channel = _persistentConnection.CreateModel())
            {

                if (!string.IsNullOrEmpty(_queueName))
                    channel.QueueUnbind(queue: _queueName,
                        exchange: BROKER_NAME,
                        routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var policy = RetryPolicy.Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(_retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (ex, time) =>
                {
                    _logger.LogWarning(ex.ToString());
                });

            using (var channel = _persistentConnection.CreateModel())
            {
                var eventName = @event.GetType()
                    .Name;

                channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                policy.Execute(() =>
                {
                    channel.BasicPublish(exchange: BROKER_NAME,
                                     routingKey: eventName,
                                     basicProperties: null,
                                     body: body);
                });
            }
        }

        public void SubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            DoInternalSubscription(eventName);
            _subsManager.AddDynamicSubscription<TH>(eventName);
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            _subsManager.AddSubscription<T, TH>();
            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);
        }

        private void DoInternalSubscription(string eventName)
        {
            //var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            //if (!containsKey)
            {
                if (!_persistentConnection.IsConnected)
                {
                    _persistentConnection.TryConnect();
                }

                using (var channel = _persistentConnection.CreateModel())
                {
                    if (!string.IsNullOrEmpty(_queueName))
                    {
                        channel.QueueBind(queue: _queueName,
                                          exchange: BROKER_NAME,
                                          routingKey: eventName);
                    }
                }
            }
        }

        public void Unsubscribe<T, TH>()
            where TH : IIntegrationEventHandler<T>
            where T : IntegrationEvent
        {
            _subsManager.RemoveSubscription<T, TH>();
        }

        public void UnsubscribeDynamic<TH>(string eventName)
            where TH : IDynamicIntegrationEventHandler
        {
            _subsManager.RemoveDynamicSubscription<TH>(eventName);
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subsManager.Clear();
        }

        private IModel CreateConsumerChannel()
        {
            if (!_persistentConnection.IsConnected)
            {
                _persistentConnection.TryConnect();
            }

            var channel = _persistentConnection.CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                 type: "direct");

            if (!string.IsNullOrEmpty(_queueName))
                channel.QueueDeclare(queue: _queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                try
                {
                    var eventName = ea.RoutingKey;
                    var message = Encoding.UTF8.GetString(ea.Body);
                    await ProcessEvent(eventName, message, ea.DeliveryTag);
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex.Message);
                }
            };

            if (!string.IsNullOrEmpty(_queueName))
                channel.BasicConsume(queue: _queueName,
                             autoAck: false,
                             consumer: consumer);


            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message, ulong deliveryTag)
        {
            try
            {
                if (_subsManager.HasSubscriptionsForEvent(eventName))
                {
                    using (var scope = _autofac.BeginLifetimeScope(AUTOFAC_SCOPE_NAME))
                    {
                        var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                        foreach (var subscription in subscriptions)
                        {
                            if (subscription.IsDynamic)
                            {
                                var handler = scope.ResolveOptional(subscription.HandlerType) as IDynamicIntegrationEventHandler;
                                dynamic eventData = JObject.Parse(message);
                                await handler.Handle(eventData);
                                _consumerChannel.BasicAck(deliveryTag, false);
                            }
                            else
                            {
                                var eventType = _subsManager.GetEventTypeByName(eventName);
                                var integrationEvent = JsonConvert.DeserializeObject(message, eventType);
                                var handler = scope.ResolveOptional(subscription.HandlerType);
                                var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);
                                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { integrationEvent });
                                _consumerChannel.BasicAck(deliveryTag, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
