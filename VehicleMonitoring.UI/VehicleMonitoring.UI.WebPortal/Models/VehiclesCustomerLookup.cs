using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleMonitoring.UI.WebPortal.Models
{
    public class VehiclesCustomerLookup
    {
        public List<CustomerLookup> CusLookup { get; set; }
        public List<CustomerVehicles> CusVehicles { get; set; }
        
    }
}
