using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.Common.Core.RestClients;
using VehicleMonitoring.Gateway.DTO;
using VehicleMonitoring.Gateway.DomainModels;
using VehicleMonitoring.Common.Messaging.Events;
using VehicleMonitoring.Common.EventBus.Interfaces;

namespace VehicleMonitoring.Gateway.Infrastructure.UnitOfWork
{
    public class VehicleServiceUOW : IVehicleServiceUOW, IDisposable
    {
        public string _customerUrl { get; set; }
        public string _customerMethodName { get; set; }

        public string _vehicleUrl { get; set; }
        public string _vehicleMethodName { get; set; }


        private ILogger<VehicleServiceUOW> _logger;
        private IEventBus _eventBus;

        public VehicleServiceUOW(ILogger<VehicleServiceUOW> logger, IEventBus eventBus)
        {
            _logger = logger;
            _eventBus = eventBus;
        }


        public List<CustomerVehicleDTO> GetCustomersVehicles(int? customerID, bool? status)
        {
            try
            {
                List<CustomerVehicleDTO> cusVehList = new List<CustomerVehicleDTO>();

                GlobalRestClient<Customer> cusrestClient = new GlobalRestClient<Customer>(_customerUrl);
                Dictionary<string, object> cusrestParam = new Dictionary<string, object>();
                if(customerID.HasValue)
                    cusrestParam.Add("customerID", customerID.Value.ToString());                
                var cusres = cusrestClient.GetWithFilter(cusrestParam, _customerMethodName).ToList();


                GlobalRestClient<Vehicle> vehrestClient = new GlobalRestClient<Vehicle>(_vehicleUrl);
                List<Vehicle> vres = new List<Vehicle>();
                foreach (Customer c in cusres)
                {
                    Dictionary<string, object> vehrestParam = new Dictionary<string, object>();
                    vehrestParam.Add("customerID", c.ID.ToString());
                    if (status.HasValue)
                        vehrestParam.Add("status", status.Value.ToString());
                   
                    vres.AddRange(vehrestClient.GetWithFilter(vehrestParam, _vehicleMethodName).ToList());
                }
                var cusVehres = from c in cusres
                           join v in vres
                           on c.ID equals v.CustomerId
                           select new  CustomerVehicle{
                               ID= c.ID,
                               Name =c.Name,
                               Address= c.Address,
                               VIN=v.VIN,
                               RegNr=v.RegNr,
                               CustomerId=v.CustomerId,
                               LastPing=v.LastPing,
                               Status= v.Status };
                
                cusVehList = CustomerVehicleDTO.GetList(cusVehres.ToList()).ToList();
                return cusVehList;

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
                GlobalRestClient<CustomerLookup> restClient = new GlobalRestClient<CustomerLookup>(_customerUrl);
                var customers=restClient.GetAll(_customerMethodName).ToList();
                customersLookups = CustomerLookupDTO.GetList(customers).ToList();
                return customersLookups;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw ex;
            }

        }
       
        public async Task UpdateVehicleStatus(string vin, DateTime lastPing)
        {
            try
            {
                
                var @eventRecieved = new VehicleStatusRecievedIntegrationEvent(vin, lastPing);
                _eventBus.Publish(@eventRecieved);
               
               
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
