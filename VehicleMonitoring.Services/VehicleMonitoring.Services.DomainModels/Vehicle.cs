using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace VehicleMonitoring.Services.DomainModels
{
    [Table("Vehicle")]
    public class Vehicle: BaseModel
    {
        
        [StringLength(50)]
        [Key]
        public string VIN { get; set; }

        [Required]
        [StringLength(50)]
        public string RegNr { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public DateTime LastPing { get; set; }

       
    }
}
