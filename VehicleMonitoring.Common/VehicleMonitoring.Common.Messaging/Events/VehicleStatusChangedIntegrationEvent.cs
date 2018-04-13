using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.Common.EventBus.Events;

namespace VehicleMonitoring.Common.Messaging.Events
{
    public class VehicleStatusChangedIntegrationEvent : IntegrationEvent
    {
        public string VIN { get; private set; }
        public DateTime LastPing { get; private set; }

        public VehicleStatusChangedIntegrationEvent(string vIN, DateTime lastPing)
        {
            this.VIN = vIN;
            this.LastPing = lastPing;
        }
    }
}