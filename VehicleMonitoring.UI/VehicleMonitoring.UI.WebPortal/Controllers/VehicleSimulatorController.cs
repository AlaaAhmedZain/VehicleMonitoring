using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VehicleMonitoring.Common.Core.RestClients;
using VehicleMonitoring.UI.WebPortal.Models;

namespace VehicleMonitoring.UI.WebPortal.Controllers
{
    public class VehicleSimulatorController : Controller
    {
        public GeneralAppSettings _config;
        public VehicleSimulatorController(IOptions<GeneralAppSettings> configOptions)
        {
            _config = configOptions.Value;
        }
        public IActionResult Index(string vin)
        {
            var _url = this._config.VehicleDashBoardURL;
            var _vmethodName = this._config.CustomersVehicleMethod;

            GlobalRestClient<CustomerVehicles> restClient = new GlobalRestClient<CustomerVehicles>(_url);
            Dictionary<string, object> restParam = new Dictionary<string, object>();
            var cusVehres = restClient.GetWithFilter(restParam, _vmethodName).ToList();

            if (!string.IsNullOrEmpty(vin))
            {
                GlobalRestClient<string> vrestClient = new GlobalRestClient<string>(_url);
                Dictionary<string, object> vrestParam = new Dictionary<string, object>();
                vrestParam.Add("vin", vin);
                _vmethodName = this._config.UpdateVehicleStatusMethod;               
                
                var res = vrestClient.Post(vrestParam, _vmethodName);


            }
            return View(cusVehres);
        }

        
    }
}