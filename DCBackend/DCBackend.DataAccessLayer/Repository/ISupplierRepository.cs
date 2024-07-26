using DCBackend.DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBackend.DataAccessLayer.Repository
{
    public interface ISupplierRepository
    {
        void AddSupplier(Supplier supplier);
        bool SupplierIdExists(Guid supplierId);
    }
}
