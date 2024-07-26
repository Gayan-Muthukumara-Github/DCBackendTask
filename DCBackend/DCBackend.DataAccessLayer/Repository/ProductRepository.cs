using DCBackend.DataAccessLayer.Entities;
using DCBackend.DataAccessLayer.Setting;
using DCBackend.DataAccessLayer.SQLHelper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBackend.DataAccessLayer.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ConnectionSetting _connection;
        private readonly ISupplierRepository _supplierRepository;

        public ProductRepository(IOptions<ConnectionSetting> connection, IHostingEnvironment env, ISupplierRepository supplierRepository)
        {
            _connection = connection.Value;
            _supplierRepository = supplierRepository;
            var path = Path.Combine(env.ContentRootPath, "Script", "ProductQueries.xml");
            SQlQueryHelper.LoadQueries(path);
        }

        public void AddProduct(Product product)
        {
            if (!ProductExists(product.ProductName))
            {
                if (!_supplierRepository.SupplierIdExists(product.SupplierId))
                {
                    throw new InvalidOperationException("The supplier does not exist.");
                }

                using (var connection = new SqlConnection(_connection.SQLString))
                {
                    var command = new SqlCommand(SQlQueryHelper.GetQuery("AddProduct"), connection);
                    command.Parameters.AddWithValue("@ProductId", product.ProductId);
                    command.Parameters.AddWithValue("@ProductName", product.ProductName);
                    command.Parameters.AddWithValue("@UnitPrice", product.UnitPrice);
                    command.Parameters.AddWithValue("@SupplierId", product.SupplierId);
                    command.Parameters.AddWithValue("@CreatedOn", product.CreatedOn);
                    command.Parameters.AddWithValue("@IsActive", product.IsActive);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                throw new InvalidOperationException("A product with the same name already exists.");
            }
        }

        public bool ProductIdExists(Guid productId)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("ProductIdExists"), connection);
                command.Parameters.AddWithValue("@ProductId", productId);
                connection.Open();
                var result = command.ExecuteScalar();
                return (result != null);
            }
        }

        private bool ProductExists(string ProductName)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("ProductExists"), connection);
                command.Parameters.AddWithValue("@ProductName", ProductName);
                connection.Open();
                var result = command.ExecuteScalar();
                return (result != null);
            }
        }
    }


}
