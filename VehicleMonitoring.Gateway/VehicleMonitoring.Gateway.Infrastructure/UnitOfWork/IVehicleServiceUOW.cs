using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleMonitoring.Common.Core.UnitOfWork;
using VehicleMonitoring.Gateway.DTO;

namespace VehicleMonitoring.Gateway.Infrastructure.UnitOfWork
{
    public interface IVehicleServiceUOW : IUnitOfWork
    {
        string _customerUrl { get; set; }
        string _customerMethodName { get; set; }

        string _vehicleUrl { get; set; }
        string _vehicleMethodName { get; set; }


        List<CustomerVehicleDTO> GetCustomersVehicles(int? customerID,bool? status);
        List<CustomerLookupDTO> GetCustomerLookup();

        Task UpdateVehicleStatus(string vin,DateTime lastPing);
    }
}
