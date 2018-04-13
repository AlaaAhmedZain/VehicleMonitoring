using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using VehicleMonitoring.Common.Messaging.Events;
using VehicleMonitoring.Gateway.Infrastructure.UnitOfWork;

namespace VehicleMonitoring.Gateway.API.Controllers
{
    [Produces("application/json")]
    [Route("api/VehicleMonitoring")]
    public class VehicleMonitoringController : Controller
    {
        private IVehicleServiceUOW _uow;
        private ILogger<CustomerVehicleSearchController> _logger;
        public GeneralAppSettings _config;
        public VehicleMonitoringController(IVehicleServiceUOW uow, ILogger<CustomerVehicleSearchController> logger, IOptions<GeneralAppSettings> configOptions)
        {
            _uow = uow;
            _logger = logger;
            _config = configOptions.Value;
        }
        /// <summary>
        /// used by the vehicles to ping thier status {true=connected  / false=disconnected}
        /// </summary>
        /// <param name="vin">vin of the vehicle</param>
        /// <returns></returns>
        [HttpPost("UpdateVehicleStatus")]
        public IActionResult UpdateVehicleStatus(string vin)
        {
            try
            {
                // Publish integration event to the event bus
                // (RabbitMQ or a service bus underneath)
                DateTime lastPing = DateTime.Now;
                _uow.UpdateVehicleStatus(vin, lastPing);
                
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex.Message);
                return StatusCode(500, new Exception("can't process your request currently. Please try again later."));
                
            }
        }
    }
}