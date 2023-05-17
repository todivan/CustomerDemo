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
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.ControllersTests
{
    [TestClass]
    public class CustomerControllerCreateTest
    {
        private static IMapper _mapper;
        private static Mock<ILogger<CustomerController>> _mockLogger;

        public CustomerControllerCreateTest()
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
        public void CustomerControlerCreate_Success()
        {
            //Arrange
            CustomerCreateDto customerCreateDto = new CustomerCreateDto();
            customerCreateDto.Firstname = "TestFirstname";
            customerCreateDto.Surename = "TestSurename";

            Customer customer = new Customer();
            customer.Id = Guid.NewGuid();
            customer.Firstname = customerCreateDto.Firstname;
            customer.Surename = customerCreateDto.Surename;

            var mockCustomerReporitory = new Mock<IRepository<Customer>>();
            mockCustomerReporitory.Setup(e => e.Add(It.Is<Customer>(d => d.Firstname == customerCreateDto.Firstname && d.Surename == customerCreateDto.Surename)))
            .Returns(customer);
            
            //Act
            CustomerController customerController = new CustomerController(mockCustomerReporitory.Object, _mockLogger.Object, _mapper);
            var result = customerController.Post(customerCreateDto);

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status200OK, objectResult?.StatusCode);
            Assert.IsInstanceOfType(objectResult?.Value, typeof(CustomerListDto));
            var createdCustomer = objectResult?.Value as CustomerListDto;
            Assert.IsNotNull(createdCustomer?.Id);
            Assert.AreEqual(customerCreateDto.Firstname, createdCustomer?.Firstname);
            Assert.AreEqual(customerCreateDto.Surename, createdCustomer?.Surename);
        }

        [TestMethod]
        public void CustomerControlerCreate_InternalServerError()
        {
            //Arrange
            CustomerCreateDto customerCreateDto = new CustomerCreateDto();
            customerCreateDto.Firstname = "TestFirstname";
            customerCreateDto.Surename = "TestSurename";

            var mockCustomerReporitory = new Mock<IRepository<Customer>>();

            //Act
            CustomerController customerController = new CustomerController(mockCustomerReporitory.Object, _mockLogger.Object, null);
            var result = customerController.Post(customerCreateDto);

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status500InternalServerError, objectResult?.StatusCode);
        }

        [TestMethod]
        public void CustomerControlerCreate_BadREquest()
        {
            //Arrange
            var mockCustomerReporitory = new Mock<IRepository<Customer>>();

            //Act
            CustomerController customerController = new CustomerController(mockCustomerReporitory.Object, _mockLogger.Object, _mapper);
            var result = customerController.Post(null);

            //Assert
            Assert.IsNotNull(result);
            var objectResult = result as ObjectResult;
            Assert.AreEqual(StatusCodes.Status400BadRequest, objectResult?.StatusCode);
        }
    }
}
