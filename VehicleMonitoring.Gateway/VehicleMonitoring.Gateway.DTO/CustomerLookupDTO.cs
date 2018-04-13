using System;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.Gateway.DomainModels;

namespace VehicleMonitoring.Gateway.DTO
{
    public class CustomerLookupDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public CustomerLookupDTO(int id, string name)
        {
            this.ID = id;
            this.Name = name;
        }

        /// <summary>
        /// Map Collection of DAL objects Into List of DTOs
        /// </summary>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public static IEnumerable<CustomerLookupDTO> GetList(ICollection<CustomerLookup> Collection)
        {
            List<CustomerLookupDTO> list = new List<CustomerLookupDTO>();
            foreach (CustomerLookup c in Collection)
            {
                list.Add(new CustomerLookupDTO(c.ID,c.Name));
            }
            return list;
        }

    }
}
