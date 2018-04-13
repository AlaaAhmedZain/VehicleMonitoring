using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.Services.DomainModels;

namespace VehicleMonitoring.Services.DTO
{
    /// <summary>
    /// Data transfer Objects, used to map and simpify DAL obejcts to the client to deliver only the data scope needed for the client 
    /// </summary>
    [Serializable]
    public class VehicleDTO
    {
       
        public string VIN { get; set; }
        public string RegNr { get; set; }
        public int CustomerId { get; set; }
        public DateTime LastPing { get; set; }
        public bool Status { get; set; }



        public VehicleDTO(Vehicle VehicleDAL)
        {
            this.VIN = VehicleDAL.VIN;
            this.RegNr = VehicleDAL.RegNr;
            this.CustomerId = VehicleDAL.CustomerId;
            this.LastPing = VehicleDAL.LastPing;
            this.Status = false;
        }
        public VehicleDTO(string vin, string regNr, int customerId, DateTime lastping,bool status)
        {
            this.VIN = vin;
            this.RegNr = regNr;
            this.CustomerId = customerId;
            this.LastPing = lastping;
            this.Status = status;
        }
        

       
        /// <summary>
        /// Map Collection of DAL objects Into List of DTOs
        /// </summary>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public static IEnumerable<VehicleDTO> GetList(ICollection<Vehicle> Collection)
        {
            List<VehicleDTO> list = new List<VehicleDTO>();
            foreach (Vehicle veh in Collection)
            {
                list.Add(new VehicleDTO(veh));
            }
            return list;
        }
       
    }
}
