using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleMonitoring.CustomerSVC.DomainModels
{
    [Table("Customer")]
    public class Customer : BaseModel
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
