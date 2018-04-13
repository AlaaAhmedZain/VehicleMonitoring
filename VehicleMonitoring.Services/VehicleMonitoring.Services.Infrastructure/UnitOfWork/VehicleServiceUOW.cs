using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.Common.Core.Repository;
using VehicleMonitoring.Services.DTO;
using VehicleMonitoring.Services.DomainModels;



namespace VehicleMonitoring.Services.Infrastructure.UnitOfWork
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


        public List<VehicleDTO> GetCustomersVehicles(int? customerID, bool? status, int TicksNO)
        {
            try
            {
                List<VehicleDTO> cusVehList = new List<VehicleDTO>();

                var vehicles = VehiclesRepo.All().Where(v => customerID == null || customerID == 0 ||v.CustomerId==customerID).ToList();
                foreach (Vehicle v in vehicles)
                {
                    bool _status = false;
                    if ((((TimeSpan)(DateTime.Now - v.LastPing)).TotalSeconds) <= TicksNO)
                    {
                        _status = true;
                    }
                    cusVehList.Add(new VehicleDTO( v.VIN, v.RegNr, v.CustomerId, v.LastPing, _status));

                }
                return cusVehList.Where(v=> status==null || v.Status==status).ToList();

                

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
