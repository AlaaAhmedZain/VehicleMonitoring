using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleMonitoring.CustomerSVC.DomainModels;

namespace VehicleMonitoring.CustomerSVC.DAL
{
    public class DbInitializer
    {
        private CustomerServiceDbContext _context;
        
        public DbInitializer(CustomerServiceDbContext context)
        {
            this._context = context;
        }

        public async Task Initialize()
        {
            if (_context == null)
            {
                return;
            }
            _context.Database.EnsureCreated();

            // Look for any customers.
            if (_context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            //you may add sample date here after database creation
            //_context.Customers or _context.Vehicles

            
        }
    }
}
