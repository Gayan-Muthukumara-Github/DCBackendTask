using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCBackend.DataAccessLayer.Entities
{
    public class ActiveOrder
    {
        public Guid OrderId { get; set; }
        public int OrderStatus { get; set; }
        public int OrderType { get; set; }
        public DateTime OrderedOn { get; set; }
        public DateTime? ShippedOn { get; set; }
        public Guid ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public Guid SupplierId { get; set; }
        public string? SupplierName { get; set; }
    }
}
