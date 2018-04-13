using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleMonitoring.Services.DomainModels
{
    public abstract class BaseModel
    {
        public DateTime? Created { get; set; }
        public string Creator { get; set; }
        public DateTime? Updated { get; set; }
        public string Updator { get; set; }
    }
}
