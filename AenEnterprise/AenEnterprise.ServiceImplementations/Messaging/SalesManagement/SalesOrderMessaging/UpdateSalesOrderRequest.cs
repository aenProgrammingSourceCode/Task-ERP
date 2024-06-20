using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging
{
    public class UpdateSalesOrderRequest
    {
        public int Id { get; set; }
        public string SalesOrderNo { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime OrderedDate { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int UnitId { get; set; }
        public string UnitName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SalesAmount { get; set; }
        public int SalesOrderStatusId { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal TotalInvoiceQuantity { get; set; }
        public decimal RemainingSalesQuantity { get; set; }
        public int CustomerId { get; set; }
    }
}
