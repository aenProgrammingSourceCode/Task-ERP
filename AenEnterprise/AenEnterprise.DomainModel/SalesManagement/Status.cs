using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class Status
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<OrderItem> OrderItems { get; set; }
        public List<InvoiceItem> InvoiceItems { get; set; }
        public List<DeliveryOrderItem> DeliveryOrderItems { get; set; }
        public List<DispatchItem> DispatchItems { get; set; }

    }
}
