using DCBackend.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBackend.DataAccessLayer.Repository
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        bool ProductIdExists(Guid productId);
    }
}
