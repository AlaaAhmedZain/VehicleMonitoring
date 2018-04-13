using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleMonitoring.Common.EventBus.Events;

namespace VehicleMonitoring.Common.EventBus.Interfaces
{
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
