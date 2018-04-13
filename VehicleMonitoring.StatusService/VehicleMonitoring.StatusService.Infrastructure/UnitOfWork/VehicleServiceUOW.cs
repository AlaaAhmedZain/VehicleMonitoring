using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.Common.Core.Repository;
using VehicleMonitoring.StatusService.DomainModels;



namespace VehicleMonitoring.StatusService.Infrastructure.UnitOfWork
{
    public class VehicleServiceUOW : IVehicleServiceUOW, IDisposable
    {
        
        protected IRepositoryProvider RepositoryProvider { get; set; }
       protected IRepository<Vehicle> VehiclesRepo { get { return GetStandardRepo<Vehicle>(); } }

         private ILogger<VehicleServiceUOW> _logger;

        public VehicleServiceUOW(IRepositoryProvider repositoryProvider, ILogger<VehicleServiceUOW> logger)
        {
            if (repositoryProvider.DbContext == null)
            {
                throw new ArgumentNullException("dbContext is null"); 
            }
            this.RepositoryProvider = repositoryProvider;
            _logger = logger;
        }


        public async Task<bool> UpdateVehicleStatus(string vin, DateTime lastPing)
        {
            try
            {
                var vehcile = VehiclesRepo.FindById(vin);
                if (vehcile != null)
                {
                    vehcile.LastPing = lastPing;
                    vehcile.Updated = DateTime.Now;
                    VehiclesRepo.Update(vehcile);
                    await VehiclesRepo.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }


        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            try
            {
                var repo = RepositoryProvider.GetRepositoryForEntityType<T>();
                return repo;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
        private T GetRepo<T>() where T : class
        {
            try
            {
                return RepositoryProvider.GetRepository<T>();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }
        
        #region Disposing 
       
        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
