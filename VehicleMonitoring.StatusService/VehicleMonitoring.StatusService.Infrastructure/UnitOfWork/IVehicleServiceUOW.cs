using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VehicleMonitoring.Common.Core.UnitOfWork;


namespace VehicleMonitoring.StatusService.Infrastructure.UnitOfWork
{
    public interface IVehicleServiceUOW : IUnitOfWork
    {
        
        Task<bool> UpdateVehicleStatus(string vin, DateTime lastPing);
    }
}
