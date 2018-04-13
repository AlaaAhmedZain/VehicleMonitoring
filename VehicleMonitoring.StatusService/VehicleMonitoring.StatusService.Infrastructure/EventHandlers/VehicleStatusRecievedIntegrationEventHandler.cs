using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VehicleMonitoring.Common.EventBus.Interfaces;
using VehicleMonitoring.Common.Messaging.Events;
using VehicleMonitoring.StatusService.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.StatusService.Infrastructure.EventHandlers
{
    public class VehicleStatusRecievedIntegrationEventHandler : IIntegrationEventHandler<VehicleStatusRecievedIntegrationEvent>
    {
       
        IVehicleServiceUOW _uow;
        private IEventBus _eventBus;
        private ILogger<VehicleStatusRecievedIntegrationEventHandler> _logger;
        
        public VehicleStatusRecievedIntegrationEventHandler(IVehicleServiceUOW uow, IEventBus eventBus, ILoggerFactory loggerFactory)
        {
            _uow = uow;
            _eventBus = eventBus;
            _logger = loggerFactory?.CreateLogger<VehicleStatusRecievedIntegrationEventHandler>();
            if (_logger == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
        }
        
        public async Task Handle(VehicleStatusRecievedIntegrationEvent @event)
        {
            try
            {
                await _uow.UpdateVehicleStatus(@event.VIN, @event.LastPing);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
    }
}
