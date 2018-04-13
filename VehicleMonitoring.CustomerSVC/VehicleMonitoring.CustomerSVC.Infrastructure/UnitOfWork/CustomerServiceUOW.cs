using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.Common.Core.Repository;
using VehicleMonitoring.CustomerSVC.DTO;
using VehicleMonitoring.CustomerSVC.DomainModels;



namespace VehicleMonitoring.CustomerSVC.Infrastructure.UnitOfWork
{
    public class CustomerServiceUOW :ICustomerServiceUOW, IDisposable
    {
        
        protected IRepositoryProvider RepositoryProvider { get; set; }
        protected IRepository<Customer> CustomersRepo { get { return GetStandardRepo<Customer>(); } }
       
        private ILogger<CustomerServiceUOW> _logger;

        public CustomerServiceUOW(IRepositoryProvider repositoryProvider, ILogger<CustomerServiceUOW> logger)
        {
            if (repositoryProvider.DbContext == null)
            {
                throw new ArgumentNullException("dbContext is null"); 
            }
            this.RepositoryProvider = repositoryProvider;
            _logger = logger;
        }


        public List<CustomerDTO> GetCustomer(int? customerID)
        {
            try
            {
                var customers = CustomersRepo.All().Where(c => customerID == null || customerID == 0 || c.ID == customerID).ToList();                
                return CustomerDTO.GetList(customers).ToList(); 

            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }
        }

        public List<CustomerLookupDTO> GetCustomerLookup()
        {
            try
            {
                List<CustomerLookupDTO> customersLookups = new List<CustomerLookupDTO>();
                var customers = CustomersRepo.All().Select(c => new { c.ID, c.Name }).ToList();
                foreach (var customer in customers)
                {
                    customersLookups.Add(new CustomerLookupDTO(customer.ID, customer.Name));
                }
                return customersLookups;
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
