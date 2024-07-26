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
    public class OrderRepository : IOrderRepository
    {
        private readonly ConnectionSetting _connection;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;

        public OrderRepository(IOptions<ConnectionSetting> connection, IHostingEnvironment env, IProductRepository productRepository, ICustomerRepository customerRepository)
        {
            _connection = connection.Value;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            var path = Path.Combine(env.ContentRootPath, "Script", "OrderQueries.xml");
            SQlQueryHelper.LoadQueries(path);
        }

        public void AddOrder(Order order)
        {
            if (_customerRepository.CustomerIdExists(order.OrderBy))
            {
                if (!_productRepository.ProductIdExists(order.ProductId))
                {
                    throw new InvalidOperationException("The product does not exist.");
                }
                else
                {
                    using (var connection = new SqlConnection(_connection.SQLString))
                    {
                        var command = new SqlCommand(SQlQueryHelper.GetQuery("AddOrder"), connection);
                        command.Parameters.AddWithValue("@OrderId", order.OrderId);
                        command.Parameters.AddWithValue("@ProductId", order.ProductId);
                        command.Parameters.AddWithValue("@OrderStatus", order.OrderStatus);
                        command.Parameters.AddWithValue("@OrderType", order.OrderType);
                        command.Parameters.AddWithValue("@OrderBy", order.OrderBy);
                        command.Parameters.AddWithValue("@OrderedOn", order.OrderedOn);
                        command.Parameters.AddWithValue("@ShippedOn", order.ShippedOn);
                        command.Parameters.AddWithValue("@IsActive", order.IsActive);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("The customer does not exist.");
            }
                
        }
    }


}
