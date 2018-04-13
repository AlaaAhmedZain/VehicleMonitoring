using System;
using System.Collections.Generic;
using System.Text;


namespace VehicleMonitoring.Gateway.DomainModels
{
    /// <summary>
    /// Data transfer Objects, used to map and simpify DAL obejcts to the client to deliver only the data scope needed for the client 
    /// </summary>
    [Serializable]
    public class CustomerVehicle
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string VIN { get; set; }
        public string RegNr { get; set; }
        public int CustomerId { get; set; }
        public DateTime LastPing { get; set; }
        public bool Status { get; set; }

        
       
        
    }
}
