using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VehicleMonitoring.Gateway.API
{
    public class EventBusAppSettings
    {
        public string QueueName { get; set; }
        public string EventBusConnection { get; set; }
        public string EventBusUserName { get; set; }
        public string EventBusPassword { get; set; }
        public string SubscriptionClientName { get; set; }
        public int EventBusRetryCount { get; set; }
    }
    public class GeneralAppSettings
    {
        public string VehicleServiceURL { get; set; }
        public string CustomerServiceURL { get; set; }
        public string GetVehicleMethod { get; set; }
        public string GetCustomerMethod { get; set; }
        public string CustomersLookupMethod { get; set; }

    }
}
