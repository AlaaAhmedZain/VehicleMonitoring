using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VehicleMonitoring.Gateway.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VehicleMonitoring.Gateway.API.Controllers
{
    [Produces("application/json")]
    [Route("api/CustomerVehicleSearch")]
    public class CustomerVehicleSearchController : Controller
    {
        private IVehicleServiceUOW _uow;
        private ILogger<CustomerVehicleSearchController> _logger;
        public GeneralAppSettings _config;
        public CustomerVehicleSearchController(IVehicleServiceUOW uow, ILogger<CustomerVehicleSearchController> logger, IOptions<GeneralAppSettings> configOptions)
        {
            _uow = uow;
            _logger = logger;
            _config = configOptions.Value;
        }
        /// <summary>
        /// Get Customer Vehicles for DashBoard via search by customer ID or status
        /// </summary>
        /// <param name="customerID">to get all customers send 0 or don't send it</param>
        /// <param name="status">to get vehicles with all status don't send status</param>
        /// <returns></returns>
        [HttpGet("GetCustomerVehicles")]
        public JsonResult GetCustomersVehicles(int? customerID = null, bool? status = null)
        {
            try
            {
                _uow._customerUrl= this._config.CustomerServiceURL;
                _uow._customerMethodName = this._config.GetCustomerMethod;

                _uow._vehicleUrl = this._config.VehicleServiceURL;
                _uow._vehicleMethodName = this._config.GetVehicleMethod;

                return Json(_uow.GetCustomersVehicles(customerID, status))  ;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
            }
        }

        /// <summary>
        /// get customer lookup for dash board in {id,name} formate
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCustomersLookup")]
        public JsonResult GetCustomersLookup()
        {
            try
            {
                _uow._customerUrl = this._config.CustomerServiceURL;
                _uow._customerMethodName = this._config.CustomersLookupMethod;

                return Json(_uow.GetCustomerLookup());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
            }
        }


    }
}