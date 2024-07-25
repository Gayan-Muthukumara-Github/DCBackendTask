using Microsoft.AspNetCore.Mvc;
using DCBackend.Models;
using System.Diagnostics;
using DCBackend.DataAccessLayer.Repository;
using DCBackend.DataAccessLayer.Entities;

namespace DCBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        [HttpGet]
        [Route("GetAllCustomers")]
        public IActionResult GetAllCustomers()
        {
            var customers = _customerRepository.GetAllCustomers();
            return Ok(customers);
        }

        [HttpPost]
        [Route("CreateCustomer")]
        public IActionResult CreateCustomer([FromBody] Customer customer)
        {
            customer.UserId = Guid.NewGuid();
            customer.CreatedOn = DateTime.UtcNow;
            _customerRepository.AddCustomer(customer);
            return CreatedAtAction(nameof(GetAllCustomers), new { id = customer.UserId }, customer);
        }

        [HttpPut("{id}")]
        [Route("UpdateCustomer/{id}")]
        public IActionResult UpdateCustomer(Guid id, [FromBody] Customer customer)
        {
            customer.UserId = id;
            _customerRepository.UpdateCustomer(customer);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Route("DeleteCustomer/{id}")]
        public IActionResult DeleteCustomer(Guid id)
        {
            _customerRepository.DeleteCustomer(id);
            return NoContent();
        }

        [HttpGet("{id}")]
        [Route("GetActiveOrdersByCustomer/{id}")]
        public IActionResult GetActiveOrdersByCustomer(Guid id)
        {
            var orders = _customerRepository.GetActiveOrdersByCustomer(id);
            return Ok(orders);
        }

    }
}
