using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DeliveryOrderMessage
{
    public class CreateDeliveryOrderRequest
    {
        public CreateDeliveryOrderRequest()
        {
            invoiceItemIdToAdd = new List<int>();
        }
        
        public IList<int> invoiceItemIdToAdd { get; set; }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal DeliveryQuantity { get; set; }
        public decimal DeliveryAmount { get; set; }
        public int InvoiceId { get; set; }
        public int SalesOrderId { get; set; }
        public int SalesOrderStatusId { get; set; }
        public int UnitId { get; set; }
        public int OrderItemId { get; set; }
        public string DeliveryOrderNo { get; set; }
        public int InvoiceItemId { get; set; }

    }
}
