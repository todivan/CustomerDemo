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
    public class CustomerControllerDeleteTests
    {
        private static IMapper _mapper;
        private static Mock<ILogger<CustomerController>> _mockLogger;

        public CustomerControllerDeleteTests()
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
        public void CustomerControlerDelete_Success()
        {
            //Arrange
            Guid id = Guid.NewGuid();
            Customer customer = new Customer();
            customer.Id = id;
            customer.Firstname = "TestFirstname";
            customer.Surename = "TestSurename";


            var mockCustomerReporitory = new Mock<IRepository<Customer>>();
            mockCustomerReporitory.Setup(e => e.Delete(It.Is<Guid>(x => x.CompareTo(id) == 0)))
            .Returns(customer);

            //Act
            CustomerController customerController = new CustomerController(mockCustomerReporitory.Object, _mockLogger.Object, _mapper);
            var result = customerController.DeleteCustomer(id);

            //Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }


        [TestMethod]
        public void CustomerControlerDelete_InternalServerError()
        {
            //Arrange
            Guid id = Guid.NewGuid();

            var mockCustomerReporitory = new Mock<IRepository<Customer>>();
            mockCustomerReporitory.Setup(e => e.Delete(It.Is<Guid>(x => x.CompareTo(id) == 0)))
            .Throws(new IOException());

            //Act
            CustomerController customerController = new CustomerController(mockCustomerReporitory.Object, _mockLogger.Object, _mapper);
            var result = customerController.DeleteCustomer(id);

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult?.StatusCode);
        }
    }
}
