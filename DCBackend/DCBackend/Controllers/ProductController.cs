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
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        [HttpPost]
        [Route("CreateProduct")]
        public HttpResponseMessage CreateProduct([FromBody] Product product)
        {
            try
            {
                product.ProductId = Guid.NewGuid();
                product.CreatedOn = DateTime.UtcNow;
                _productRepository.AddProduct(product);

                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("Product created successfully.")
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