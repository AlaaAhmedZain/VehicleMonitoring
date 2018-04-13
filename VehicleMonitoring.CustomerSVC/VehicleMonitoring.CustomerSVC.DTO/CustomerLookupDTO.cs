using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleMonitoring.CustomerSVC.DTO
{
    [Serializable]
    public class CustomerLookupDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public CustomerLookupDTO(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        
    }
}
