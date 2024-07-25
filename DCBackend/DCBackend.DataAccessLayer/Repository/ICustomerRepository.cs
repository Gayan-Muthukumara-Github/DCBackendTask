using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DCBackend.DataAccessLayer.Entities;

namespace DCBackend.DataAccessLayer.Repository
{
    public interface ICustomerRepository
    {
        void AddCustomer(Customer customer);
        IEnumerable<Customer> GetAllCustomers();
        Customer GetCustomerById(Guid userId);
        void UpdateCustomer(Customer customer);
        void DeleteCustomer(Guid userId);
        IEnumerable<ActiveOrder> GetActiveOrdersByCustomer(Guid userId);
    }
}
