using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VehicleMonitoring.CustomerSVC.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.CustomerSVC.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : Controller
    {
        private ICustomerServiceUOW _uow;
        private ILogger<CustomersController> _logger;
        public GeneralAppSettings _config;

        public CustomersController(ICustomerServiceUOW uow, ILogger<CustomersController> logger, IOptions<GeneralAppSettings> configOptions)
        {
            _uow = uow;
            _logger = logger;
            _config = configOptions.Value;
        }
        /// <summary>
        /// Get the Customers Lookup {ID,Name}
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetCustomersLookup")]
        public JsonResult GetCustomersLookup()
        {
            try
            {
                return Json(_uow.GetCustomerLookup());
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
            }
        }
        /// <summary>
        /// Get Customer By ID and get all Customers if customerID=0 or customerID not sent
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet("GetCustomer")]
        public JsonResult GetCustomer(int? customerID)
        {
            try
            {
                return Json(_uow.GetCustomer(customerID));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
            }
        }
    }
}