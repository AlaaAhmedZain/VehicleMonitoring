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
using VehicleMonitoring.Services.DAL;
using VehicleMonitoring.Services.DomainModels;
using VehicleMonitoring.Services.DTO;
using VehicleMonitoring.Services.Infrastructure.UnitOfWork;

namespace UnitTest.Gateway
{
    [TestClass]
    public class VehicleSearchTest
    {
        private static List<VehicleDTO> _customersLookups;

        private static ILogger<VehicleServiceUOW> _mockLogger;
        private static VehicleServiceDbContext _mockServiceDbContext;
        private static Mock<IRepositoryProvider> _mockRepositoryProvider;
        private static Mock<IRepository<Vehicle>> _mockCustomersRepo;

        private static VehicleServiceUOW _vehicleServiceUow;

        [ClassInitialize]
        public static void BeforeAllTests(TestContext context)
        {
            _mockLogger = Mock.Of<ILogger<VehicleServiceUOW>>();
            var optionsBuilder = new DbContextOptionsBuilder<VehicleServiceDbContext>();
            _mockServiceDbContext = new VehicleServiceDbContext(optionsBuilder.Options);

            _mockRepositoryProvider = new Mock<IRepositoryProvider>();
            _mockCustomersRepo = new Mock<IRepository<Vehicle>>();

            FillVehiclesListSample();
            _mockRepositoryProvider.Setup(rep => rep.DbContext).Returns(_mockServiceDbContext);
        }
        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockCustomersRepo = new Mock<IRepository<Vehicle>>();
        }


        private static List<Vehicle> _vehicles;
        private static void FillVehiclesListSample()
        {
            _vehicles = new List<Vehicle>();
            _vehicles.AddRange(new List<Vehicle>() {
                new Vehicle { VIN = "YS2R4X20005399401", RegNr = "ABC123", CustomerId = 1,  LastPing = DateTime.Now, Created = DateTime.Now },
                new Vehicle { VIN = "VLUR4X20009093588", RegNr = "DEF456", CustomerId = 1, LastPing = DateTime.Now, Created = DateTime.Now },
                new Vehicle { VIN = "VLUR4X20009048066", RegNr = "GHI789", CustomerId = 1, LastPing = DateTime.Now, Created = DateTime.Now },

                 new Vehicle { VIN = "YS2R4X20005388011", RegNr = "JKL012", CustomerId = 2,  LastPing = DateTime.Now, Created = DateTime.Now },
                new Vehicle { VIN = "YS2R4X20005387949", RegNr = "MNO345", CustomerId = 2, LastPing = DateTime.Now, Created = DateTime.Now },
                new Vehicle { VIN = "YS2R4X20005387765", RegNr = "PQR678", CustomerId = 3, LastPing = DateTime.Now, Created = DateTime.Now },

                 new Vehicle { VIN = "YS2R4X20005387055", RegNr = "STU901", CustomerId = 3,  LastPing = DateTime.Now, Created = DateTime.Now },
               

            });
        }

        [TestMethod]
        public void  GetAllCustomersVehicles()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_vehicles.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Vehicle>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            // Act
            var result = _vehicleServiceUow.GetCustomersVehicles(null,null,60);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<VehicleDTO>));
            Assert.AreEqual(7, result.Count);
        }

        [TestMethod]
        public void GetCustomerVehiclesByCusID()
        {
            // Arrange
            _mockCustomersRepo.Setup(rep => rep.All()).Returns(_vehicles.AsQueryable());
            _mockRepositoryProvider.Setup(rep => rep.GetRepositoryForEntityType<Vehicle>()).Returns(_mockCustomersRepo.Object);
            _vehicleServiceUow = new VehicleServiceUOW(_mockRepositoryProvider.Object, _mockLogger);

            int cus_id = 1;

            // Act
            var result = _vehicleServiceUow.GetCustomersVehicles(cus_id,null, 60);

            // Assert
            Assert.IsInstanceOfType(result, typeof(IEnumerable<VehicleDTO>));
           string vins= _vehicles.Where(v=>v.CustomerId==1).Select(i => i.VIN).Aggregate((i, j) => i + "," + j);
            string resvins = result.Select(i => i.VIN).Aggregate((i, j) => i + "," + j);
            
            Assert.AreEqual(vins, resvins);
        }

    }
}
