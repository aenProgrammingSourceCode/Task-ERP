using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.DispatchOrder
{
    public class CreateDispatchRequest
    {
        public CreateDispatchRequest()
        {
            deliveryItemToAdd = new List<int>();
        }
        public IList<int> deliveryItemToAdd { get; set; }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string VehicalNo { get; set; } = string.Empty;
        public decimal VehicalEmptyWeight { get; set; }
        public decimal VehicalLoadWeight { get; set; }
        public decimal DispatchQuantity { get; set; }
        public int UnitId { get; set; }
        public int SalesOrderId { get; set; }
        public int DeliveryOrderId { get; set; }
        public int DeliveryOrderItemId { get; set; }
        public int InvoiceId { get; set; }
        public int ProductId { get; set; }
        public int OrderItemId { get; set; }
    }
}
