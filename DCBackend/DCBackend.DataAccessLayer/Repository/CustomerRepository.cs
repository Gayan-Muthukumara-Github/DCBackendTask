using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCBackend.DataAccessLayer.Entities;
using DCBackend.DataAccessLayer.Setting;
using DCBackend.DataAccessLayer.SQLHelper;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Hosting;

namespace DCBackend.DataAccessLayer.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ConnectionSetting _connection;

        public CustomerRepository(IOptions<ConnectionSetting> connection, IHostingEnvironment env)
        {
            _connection = connection.Value;
            var path = Path.Combine(env.ContentRootPath, "Script", "CustomerQueries.xml");
            SQlQueryHelper.LoadQueries(path);
        }

        public void AddCustomer(Customer customer)
        {
            if (!CustomerExists(customer.Email, customer.Username))
            {
                using (var connection = new SqlConnection(_connection.SQLString))
                {
                    var command = new SqlCommand(SQlQueryHelper.GetQuery("AddCustomer"), connection);
                    command.Parameters.AddWithValue("@UserId", customer.UserId);
                    command.Parameters.AddWithValue("@Username", customer.Username);
                    command.Parameters.AddWithValue("@Email", customer.Email);
                    command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                    command.Parameters.AddWithValue("@LastName", customer.LastName);
                    command.Parameters.AddWithValue("@CreatedOn", customer.CreatedOn);
                    command.Parameters.AddWithValue("@IsActive", customer.IsActive);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                throw new InvalidOperationException("A customer with the same email or username already exists.");
            }
        }

        private bool CustomerExists(string email, string username)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("CustomerExists"), connection);
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@Username", username);
                connection.Open();
                var result = command.ExecuteScalar();
                return (result != null);
            }
        }

        public bool CustomerIdExists(Guid userId)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("CustomerIdExists"), connection);
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                var result = command.ExecuteScalar();
                return (result != null);
            }
        }


        public void DeleteCustomer(Guid userId)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("DeleteCustomer"), connection);
                command.Parameters.AddWithValue("@UserId", userId);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            var customers = new List<Customer>();
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("GetAllCustomers"), connection);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customers.Add(new Customer
                        {
                            UserId = reader.GetGuid(0),
                            Username = reader.GetString(1),
                            Email = reader.GetString(2),
                            FirstName = reader.GetString(3),
                            LastName = reader.GetString(4),
                            CreatedOn = reader.GetDateTime(5),
                            IsActive = reader.GetBoolean(6)
                        });
                    }
                }
            }
            return customers;
        }

        public void UpdateCustomer(Customer customer)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("UpdateCustomer"), connection);
                command.Parameters.AddWithValue("@UserId", customer.UserId);
                command.Parameters.AddWithValue("@Username", customer.Username);
                command.Parameters.AddWithValue("@Email", customer.Email);
                command.Parameters.AddWithValue("@FirstName", customer.FirstName);
                command.Parameters.AddWithValue("@LastName", customer.LastName);
                command.Parameters.AddWithValue("@IsActive", customer.IsActive);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public IEnumerable<ActiveOrder> GetActiveOrdersByCustomer(Guid userId)
        {
            var orders = new List<ActiveOrder>();
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand("GetActiveOrdersByCustomer", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                command.Parameters.AddWithValue("@USERID", userId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        orders.Add(new ActiveOrder
                        {
                            OrderId = reader.GetGuid(0),
                            OrderStatus = reader.GetInt32(1),
                            OrderType = reader.GetInt32(2),
                            OrderedOn = reader.GetDateTime(3),
                            ShippedOn = reader.IsDBNull(4) ? (DateTime?)null : reader.GetDateTime(4),
                            ProductId = reader.GetGuid(5),
                            ProductName = reader.GetString(6),
                            UnitPrice = reader.GetDecimal(7),
                            SupplierId = reader.GetGuid(8),
                            SupplierName = reader.GetString(9)
                        });
                    }
                }
            }
            return orders;
        }
    }
}
