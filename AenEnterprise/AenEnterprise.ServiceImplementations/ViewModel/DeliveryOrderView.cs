using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel
{
    public class DeliveryOrderView
    {
        public DeliveryOrderView()
        {
            DeliveryOrderItems = new List<DeliveryOrderItemView>();
        }
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int SalesOrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<DeliveryOrderItemView> DeliveryOrderItems { get; set; }
        public string deliveryOrderNo { get; set; }
    }
}
