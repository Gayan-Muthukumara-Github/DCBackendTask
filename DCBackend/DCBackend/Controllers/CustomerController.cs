using Microsoft.AspNetCore.Mvc;
using DCBackend.Models;
using System.Diagnostics;
using DCBackend.DataAccessLayer.Repository;
using DCBackend.DataAccessLayer.Entities;
using System.Net;
using System.Net.Http;


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
        public HttpResponseMessage CreateCustomer([FromBody] Customer customer)
        {
            try
            {
                customer.UserId = Guid.NewGuid();
                customer.CreatedOn = DateTime.UtcNow;
                _customerRepository.AddCustomer(customer);

                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("Customer created successfully.")
                };
                return response;
            }
            catch (InvalidOperationException ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.Conflict)
                {
                    Content = new StringContent(ex.Message)
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred: {ex.Message}")
                };
                return response;
            }
        }


        [HttpPut("{id}")]
        [Route("UpdateCustomer/{id}")]
        public HttpResponseMessage UpdateCustomer(Guid id, [FromBody] Customer customer)
        {
            try
            {
                customer.UserId = id;
                _customerRepository.UpdateCustomer(customer);

                var response = new HttpResponseMessage(HttpStatusCode.NoContent)
                {
                    Content = new StringContent("Customer updated successfully.")
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred: {ex.Message}")
                };
                return response;
            }
        }

        [HttpDelete("{id}")]
        [Route("DeleteCustomer/{id}")]
        public HttpResponseMessage DeleteCustomer(Guid id)
        {
            try
            {
                _customerRepository.DeleteCustomer(id);

                var response = new HttpResponseMessage(HttpStatusCode.NoContent)
                {
                    Content = new StringContent("Customer deleted successfully.")
                };
                return response;
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent($"An error occurred: {ex.Message}")
                };
                return response;
            }
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
