using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleMonitoring.Common.Core.UnitOfWork;
using VehicleMonitoring.CustomerSVC.DTO;

namespace VehicleMonitoring.CustomerSVC.Infrastructure.UnitOfWork
{
    public interface ICustomerServiceUOW : IUnitOfWork
    {
        List<CustomerDTO> GetCustomer(int? customerID);
        List<CustomerLookupDTO> GetCustomerLookup();
       
    }
}
