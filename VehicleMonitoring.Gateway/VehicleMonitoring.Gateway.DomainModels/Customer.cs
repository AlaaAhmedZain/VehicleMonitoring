using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace VehicleMonitoring.Gateway.DomainModels
{
   [Serializable]
    public class Customer
    {
       

        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Address { get; set; }

    }
}
