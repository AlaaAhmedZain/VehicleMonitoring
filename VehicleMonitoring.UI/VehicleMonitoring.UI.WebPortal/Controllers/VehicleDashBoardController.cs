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
    public class VehicleDashBoardController : Controller
    {
        public GeneralAppSettings _config;
        public VehicleDashBoardController(IOptions<GeneralAppSettings> configOptions)
        {
            _config = configOptions.Value;
        }
        public IActionResult Index(int? customerID , bool? status)
        {
            var _url = this._config.VehicleDashBoardURL;
            var _vmethodName = this._config.CustomersVehicleMethod;
            var _cmethodName = this._config.CustomersLookupMethod;

            GlobalRestClient<CustomerLookup> cusRestClient = new GlobalRestClient<CustomerLookup>(_url);
            Dictionary<string, object> cusRestParam = new Dictionary<string, object>();
            var cusres = cusRestClient.GetWithFilter(cusRestParam, _cmethodName).ToList();


            GlobalRestClient<CustomerVehicles> restClient = new GlobalRestClient<CustomerVehicles>(_url);
            Dictionary<string, object> restParam = new Dictionary<string, object>();
            if (customerID.HasValue)
            restParam.Add("customerID", customerID.Value.ToString());
            if (status.HasValue)
                restParam.Add("status", status.Value.ToString());
            var cusVehres = restClient.GetWithFilter(restParam, _vmethodName).ToList();

            VehiclesCustomerLookup vcl = new VehiclesCustomerLookup();
            vcl.CusLookup = cusres;
            vcl.CusVehicles = cusVehres;

           
            return View(vcl);
        }

      
    }
}