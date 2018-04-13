using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VehicleMonitoring.CustomerSVC.DomainModels;

namespace VehicleMonitoring.CustomerSVC.DAL
{
    public class CustomerServiceDbContext : DbContext
    {
        public CustomerServiceDbContext(DbContextOptions<CustomerServiceDbContext> options) : base(options)
        {

        }

        public virtual DbSet<Customer> Customers { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Customer>()
                .Property(e => e.Name)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.Address)
                .IsUnicode(false);

        }
    }
}