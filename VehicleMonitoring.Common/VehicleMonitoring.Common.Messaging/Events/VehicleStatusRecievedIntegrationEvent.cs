using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.Common.EventBus.Events;

namespace VehicleMonitoring.Common.Messaging.Events
{
    public class VehicleStatusRecievedIntegrationEvent : IntegrationEvent
    {
        
        public string VIN { get; private set; }
        public DateTime LastPing { get; private set; }
        
        
        public VehicleStatusRecievedIntegrationEvent(string vIN, DateTime lastPing)
        {
            this.VIN = vIN;
            this.LastPing = lastPing;
        }
        
    }
}
