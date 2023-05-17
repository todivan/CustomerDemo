using AutoMapper;
using CustomerDemo;
using CustomerDemo.Controllers;
using CustomerDemo.DB;
using CustomerDemo.Dto;
using CustomerDemo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ControllersTests
{
    [TestClass]
    public class CustomerControllerGetAllTests
    {
        private static IMapper _mapper;
        private static Mock<ILogger<CustomerController>> _mockLogger;

        public CustomerControllerGetAllTests()
        {
            if (_mapper == null)
            {
                var mappingConfig = new MapperConfiguration(mc =>
                {
                    mc.AddProfile(new MapperConfig());
                });
                IMapper mapper = mappingConfig.CreateMapper();
                _mapper = mapper;
            }

            _mockLogger = new Mock<ILogger<CustomerController>>();
        }

        [TestMethod]
        public void CustomerControlerGetAll_Success()
        {
            //Arrange
            Customer customer1 = new Customer();
            customer1.Id = Guid.NewGuid();
            customer1.Firstname = "TestFirstname1";
            customer1.Surename = "TestSurename1";

            Customer customer2 = new Customer();
            customer2.Id = Guid.NewGuid();
            customer2.Firstname = "TestFirstname2";
            customer2.Surename = "TestSurename2";

            List<Customer> customers = new List<Customer>() { customer1, customer2 };

            var mockCustomerReporitory = new Mock<IRepository<Customer>>();
            mockCustomerReporitory.Setup(e => e.GetAll())
            .Returns(customers);

            //Act
            CustomerController customerController = new CustomerController(mockCustomerReporitory.Object, _mockLogger.Object, _mapper);
            var result = customerController.Get();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual(customer1.Id, result.ElementAt(0).Id);
            Assert.AreEqual(customer1.Firstname, result.ElementAt(0).Firstname);
            Assert.AreEqual(customer1.Surename, result.ElementAt(0).Surename);
            Assert.AreEqual(customer2.Id, result.ElementAt(1).Id);
            Assert.AreEqual(customer2.Firstname, result.ElementAt(1).Firstname);
            Assert.AreEqual(customer2.Surename, result.ElementAt(1).Surename);
        }

        [TestMethod]
        public void CustomerControlerGetAll_Exception()
        {
            List<Customer> customers = new List<Customer>();

            var mockCustomerReporitory = new Mock<IRepository<Customer>>();

            bool success = false;

            //Act
            try
            {
                CustomerController customerController = new CustomerController(mockCustomerReporitory.Object, _mockLogger.Object, null);
                var result = customerController.Get();
            } 
            catch (Exception)
            {
                success = true;
            }

            //Assert
            Assert.IsTrue(success);
        }
    }
}
