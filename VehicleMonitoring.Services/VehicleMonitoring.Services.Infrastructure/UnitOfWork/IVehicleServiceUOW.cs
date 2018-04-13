using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleMonitoring.Common.Core.UnitOfWork;
using VehicleMonitoring.Services.DTO;

namespace VehicleMonitoring.Services.Infrastructure.UnitOfWork
{
    public interface IVehicleServiceUOW : IUnitOfWork
    {
        List<VehicleDTO> GetCustomersVehicles(int? customerID, bool? status,int TicksNO);
        
    }
}
