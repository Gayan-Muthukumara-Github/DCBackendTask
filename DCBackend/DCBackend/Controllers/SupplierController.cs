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
    public class SupplierController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        [HttpPost]
        [Route("CreateSupplier")]
        public HttpResponseMessage CreateSupplier([FromBody] Supplier supplier)
        {
            try
            {
                supplier.SupplierId = Guid.NewGuid();
                supplier.CreatedOn = DateTime.UtcNow;
                _supplierRepository.AddSupplier(supplier);

                var response = new HttpResponseMessage(HttpStatusCode.Created)
                {
                    Content = new StringContent("Supplier created successfully.")
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

