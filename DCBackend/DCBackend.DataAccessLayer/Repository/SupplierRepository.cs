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
    public class SupplierRepository : ISupplierRepository
    {
        private readonly ConnectionSetting _connection;

        public SupplierRepository(IOptions<ConnectionSetting> connection, IHostingEnvironment env)
        {
            _connection = connection.Value;
            var path = Path.Combine(env.ContentRootPath, "Script", "SupplierQueries.xml");
            SQlQueryHelper.LoadQueries(path);
        }

        public void AddSupplier(Supplier supplier)
        {
            if (!SupplierExists(supplier.SupplierName))
            {
                using (var connection = new SqlConnection(_connection.SQLString))
                {
                    var command = new SqlCommand(SQlQueryHelper.GetQuery("AddSupplier"), connection);
                    command.Parameters.AddWithValue("@SupplierId", supplier.SupplierId);
                    command.Parameters.AddWithValue("@SupplierName", supplier.SupplierName);
                    command.Parameters.AddWithValue("@CreatedOn", supplier.CreatedOn);
                    command.Parameters.AddWithValue("@IsActive", supplier.IsActive);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            else
            {
                throw new InvalidOperationException("A supplier with the same name already exists.");
            }
        }

        public bool SupplierIdExists(Guid supplierId)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("SupplierIdExists"), connection);
                command.Parameters.AddWithValue("@SupplierId", supplierId);
                connection.Open();
                var result = command.ExecuteScalar();
                return (result != null);
            }
        }

        private bool SupplierExists(string SupplierName)
        {
            using (var connection = new SqlConnection(_connection.SQLString))
            {
                var command = new SqlCommand(SQlQueryHelper.GetQuery("SupplierExists"), connection);
                command.Parameters.AddWithValue("@SupplierName", SupplierName);
                connection.Open();
                var result = command.ExecuteScalar();
                return (result != null);
            }
        }
    }

    
}
