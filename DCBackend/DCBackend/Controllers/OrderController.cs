using DCBackend.DataAccessLayer.Entities;
using DCBackend.DataAccessLayer.Repository;
using Microsoft.AspNetCore.Mvc;
using DCBackend.Models;
using System.Diagnostics;
using System.Net;
using System.Net.Http;


namespace DCBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [HttpPost]
        [Route("CreateOrder")]
        public HttpResponseMessage CreateOrder([FromBody] Order order)
        {
            try
            {
                order.OrderId = Guid.NewGuid();
                order.OrderedOn = DateTime.UtcNow;
                _orderRepository.AddOrder(order);

                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("Order created successfully.")
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


    }
}
