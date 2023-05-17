using AutoMapper;
using CustomerDemo.DB;
using CustomerDemo.Dto;
using CustomerDemo.Model;
using Microsoft.AspNetCore.Mvc;

namespace CustomerDemo.Controllers
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IRepository<Customer> _repository;
        private readonly IMapper _mapper;

        public CustomerController(IRepository<Customer> customerRepository, ILogger<CustomerController> logger, IMapper mapper)
        {
            _logger = logger;
            _repository = customerRepository;
            _mapper = mapper;
        }

        [HttpPost(Name = "CreateCustomer")]
        public IActionResult Post([FromBody] CustomerCreateDto customer)
        {
            try
            {
                _logger.LogInformation("Creating new customer");

                if (customer is null)
                {
                    _logger.LogError("customer object sent from client is null.");
                    return BadRequest("customer object is null");
                }
                if (!ModelState.IsValid)
                {
                    _logger.LogError("Invalid customer object sent from client.");
                    return BadRequest("Invalid model object");
                }

                var customerEntity = _mapper.Map<Customer>(customer);
                customerEntity = _repository.Add(customerEntity);
                
                var createdCustomer = _mapper.Map<CustomerListDto>(customerEntity);
                return Ok(createdCustomer);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside CreateCustomer action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet(Name = "GetAllCustomers")]
        public IEnumerable<CustomerListDto> Get()
        {
            try
            {
                _logger.LogInformation("Getting all customers");
                var allCustomers = _repository.GetAll();
                var allCustomersDto = _mapper.Map<List<CustomerListDto>>(allCustomers);
                return allCustomersDto;
            }
            catch
            (Exception ex)
            {
                _logger.LogError($"Something went wrong inside GetAllCustomers action: {ex.Message}");
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id}", Name = "DeleteCustomer")]
        public IActionResult DeleteCustomer(Guid id)
        {
            try
            {
                _logger.LogInformation($"Deleting customer ID={id}");
                var deletedCustomer = _repository.Delete(id);
                if(deletedCustomer == null)
                {
                    _logger.LogError($"Customer with id: {id}, hasn't been found in db.");
                    return NotFound();
                }

                return Ok();
            }
            catch
            (Exception ex)
            {
                _logger.LogError($"Something went wrong inside DeleteCustomer action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
