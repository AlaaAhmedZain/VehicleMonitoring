using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.Gateway.DomainModels;


namespace VehicleMonitoring.Gateway.DTO
{
    /// <summary>
    /// Data transfer Objects, used to map and simpify DAL obejcts to the client to deliver only the data scope needed for the client 
    /// </summary>
    [Serializable]
    public class CustomerVehicleDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string VIN { get; set; }
        public string RegNr { get; set; }
        public int CustomerId { get; set; }
        public DateTime LastPing { get; set; }
        public bool Status { get; set; }

        public CustomerVehicleDTO(CustomerVehicle VehicleDAL)
        {
            this.VIN = VehicleDAL.VIN;
            this.RegNr = VehicleDAL.RegNr;
            this.CustomerId = VehicleDAL.CustomerId;
            this.LastPing = VehicleDAL.LastPing;
            this.ID = VehicleDAL.ID;
            this.Name = VehicleDAL.Name;
            this.Address = VehicleDAL.Address;
            this.Status = VehicleDAL.Status;

        }
        public CustomerVehicleDTO(int id,string name,string address, string vin, string regNr, int customerId, DateTime lastping,bool status)
        {
            this.VIN = vin;
            this.RegNr = regNr;
            this.CustomerId = customerId;
            this.LastPing = lastping;
            this.ID = id;
            this.Name = name;
            this.Address = address;
            this.Status = status;


        }

        /// <summary>
        /// Map Collection of DAL objects Into List of DTOs
        /// </summary>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public static IEnumerable<CustomerVehicleDTO> GetList(ICollection<CustomerVehicle> Collection)
        {
            List<CustomerVehicleDTO> list = new List<CustomerVehicleDTO>();
            foreach (CustomerVehicle veh in Collection)
            {
                list.Add(new CustomerVehicleDTO(veh));
            }
            return list;
        }


    }
}
