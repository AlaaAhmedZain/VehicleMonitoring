using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleMonitoring.UI.WebPortal.Models
{
    [Serializable]
    public class CustomerVehicles 
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
