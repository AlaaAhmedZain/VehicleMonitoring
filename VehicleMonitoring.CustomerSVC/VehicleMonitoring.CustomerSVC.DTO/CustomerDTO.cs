using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.CustomerSVC.DomainModels;

namespace VehicleMonitoring.CustomerSVC.DTO
{
    /// <summary>
    /// Data transfer Objects, used to map and simpify DAL obejcts to the client to deliver only the data scope needed for the client 
    /// </summary>
    [Serializable]
    public class CustomerDTO
    {
        
        public int ID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
       
       
        /// <summary>
        /// map DAL customer to DTO customer
        /// </summary>
        /// <param name="customerDAL"></param>
        public CustomerDTO(Customer customerDAL)
        {
            this.ID = customerDAL.ID;
            this.Name = customerDAL.Name;
            this.Address = customerDAL.Address;
            
        }
        public CustomerDTO(int id, string name, string address)
        {
            this.ID = id;
            this.Name = name;
            this.Address = address;
        }
       
        /// <summary>
        /// Map Collection of DAL objects Into List of DTOs
        /// </summary>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public static IEnumerable<CustomerDTO> GetList(ICollection<Customer> Collection)
        {
            List<CustomerDTO> list = new List<CustomerDTO>();
            foreach (Customer cus in Collection)
            {
                list.Add(new CustomerDTO(cus));
            }
            return list;
        }
        
       

    }
}
