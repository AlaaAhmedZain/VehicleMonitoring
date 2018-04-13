using System;
using System.Collections.Generic;
using System.Text;


namespace VehicleMonitoring.Gateway.DomainModels
{
    
    [Serializable]
    public class Vehicle
    {
       
        public string VIN { get; set; }
        public string RegNr { get; set; }
        public int CustomerId { get; set; }
        public DateTime LastPing { get; set; }
        public bool Status { get; set; }

        
    }
}
