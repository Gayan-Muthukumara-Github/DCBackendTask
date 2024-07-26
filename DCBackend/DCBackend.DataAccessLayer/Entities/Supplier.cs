using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBackend.DataAccessLayer.Entities
{
    public class Supplier
    {
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
