using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.Services.DomainModels;

namespace VehicleMonitoring.Services.DAL
{
    public class VehicleServiceDbContext : DbContext
    {
        public VehicleServiceDbContext(DbContextOptions<VehicleServiceDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Vehicle> Vehicles { get; set; }

       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.VIN)
                .IsUnicode(false);

            modelBuilder.Entity<Vehicle>()
                .Property(e => e.RegNr)
                .IsUnicode(false);
        }
    }
}