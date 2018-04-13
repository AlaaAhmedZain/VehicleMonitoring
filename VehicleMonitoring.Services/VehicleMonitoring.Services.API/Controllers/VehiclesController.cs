using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VehicleMonitoring.Services.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.Services.API.Controllers
{
    [Produces("application/json")]
    [Route("api/Vehicles")]
    public class VehiclesController : Controller
    {
        private IVehicleServiceUOW _uow;
        private ILogger<VehiclesController> _logger;
        public GeneralAppSettings _config;

        public VehiclesController(IVehicleServiceUOW uow, ILogger<VehiclesController> logger, IOptions<GeneralAppSettings> configOptions)
        {
            _uow = uow;
            _logger = logger;
            _config = configOptions.Value;
        }
        /// <summary>
        /// Get Vehicles search by customer ID or vehicle status
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("GetCustomerVehicles")]
        public JsonResult GetCustomersVehicles(int? customerID=null, bool? status=null)
        {
            try
            {
                var ticksNO = this._config.TicksNO;
                return Json(_uow.GetCustomersVehicles(customerID, status, ticksNO));
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                throw new InvalidOperationException("Ops! we can't process your request currently. Please try again later.");
            }
        }
    }

}
