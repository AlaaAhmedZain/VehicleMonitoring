using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleMonitoring.StatusService.DomainModels;

namespace VehicleMonitoring.StatusService.DAL
{
    public class DbInitializer
    {
        private VehicleServiceDbContext _context;
        
        public DbInitializer(VehicleServiceDbContext context)
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

            // Look for any vehicles.
            if (_context.Vehicles.Any())
            {
                return;   // DB has been seeded
            }

            //you may add sample date here after database creation
            //_context.Customers or _context.Vehicles

            
        }
    }
}
