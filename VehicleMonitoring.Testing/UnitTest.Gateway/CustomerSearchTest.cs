using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using VehicleMonitoring.Common.Core.Repository;
using VehicleMonitoring.CustomerSVC.DAL;
using VehicleMonitoring.CustomerSVC.DomainModels;
using VehicleMonitoring.CustomerSVC.DTO;
using VehicleMonitoring.CustomerSVC.Infrastructure.UnitOfWork;

namespace UnitTest.Gateway
{
    [TestClass]
    public class CustomerSearchTest 
    {
        private static List<CustomerLookupDTO> _customersLookups;
        
        private static ILogger<CustomerServiceUOW> _mockLogger;
        private static CustomerServiceDbContext  _mockServiceDbContext;
        private static Mock<IRepositoryProvider> _mockRepositoryProvider;
        private static Mock<IRepository<Customer>> _mockCustomersRepo;

        private static CustomerServiceUOW _vehicleServiceUow;


        [ClassInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            _mockLogger = Mock.Of<ILogger<CustomerServiceUOW>>();
            var optionsBuilder = new DbContextOptionsBuilder<CustomerServiceDbContext>();
            _mockServiceDbContext = new CustomerServiceDbContext(optionsBuilder.Options);

            _mockRepositoryProvider = new Mock<IRepositoryProvider>();
            _mockCustomersRepo = new Mock<IRepository<Customer>>();

            FillCustomersListSample();
            _mockRepositoryProvider.Setup(rep => rep.DbContext).Returns(_mockServiceDbContext);
        }
        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockCustomersRepo = new Mock<IRepository<Customer>>();
        }


        private static List<Customer> _customers;
        private static void FillCustomersListSample()
        {
            _customers = new List<Customer>();
            _customers.AddRange(new List<Customer>() {
               new Customer { ID = 1, Name = "Kalles Grustransporter AB", Address = "Cementvägen 8, 111 11 Södertälje",  Created = DateTime.Now },
               new Customer { ID = 2, Name = "Johans Bulk AB", Address = "Balkvägen 12, 222 22 Stockholm", Created = DateTime.Now },
               new Customer { ID =3, Name = "Haralds Värdetransporter AB", Address = "Budgetvägen 1, 333 33 Uppsala",  Created = DateTime.Now }
               });
        }

        [TestMethod]
        public void GetCustomersLookup()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customers.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new CustomerServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomerLookup();

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<CustomerLookupDTO>));
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void GetAllCustomers()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customers.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new CustomerServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomer(null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<CustomerDTO>));
            Assert.AreEqual(3, result.Count);
        }

        [TestMethod]
        public void GetCustomerByCusID()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_customers.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Customer>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new CustomerServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            int id = 1;
            string name = "Kalles Grustransporter AB";
            // Act
            var result = _vehicleServiceUow.GetCustomer(id);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<CustomerDTO>));
            Assert.AreEqual(name.ToLower(), result[0].Name.ToLower());
        }
    }
}
