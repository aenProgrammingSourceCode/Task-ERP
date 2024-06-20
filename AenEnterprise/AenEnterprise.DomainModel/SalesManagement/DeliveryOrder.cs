using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class DeliveryOrder
    {
        private IList<DeliveryOrderItem> _deliveryOrderItem;
        private string _deliveryOrderNo;
        public DeliveryOrder()
        {
            _deliveryOrderItem = new List<DeliveryOrderItem>();
            CreatedDate = DateTime.Today;
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SalesOrderId { get; set; }
        public int InvoiceId { get; set; }
        public SalesOrder SalesOrder { get; set; }
        public Invoice Invoice { get; set; }
        
        public List<DispatcheOrder> DispatcheOrders { get; set; }
        public string DeliveryOrderNo { get => _deliveryOrderNo; set => _deliveryOrderNo = value; }
        public IEnumerable<DeliveryOrderItem> DeliveryOrderItem { get => _deliveryOrderItem;}

        public void CreateDeliveryOrderItem(OrderItem orderItem, decimal deliveryQuantity)
        {
            _deliveryOrderItem.Add(DeliveryOrderItemFactory.CreateDeliveryOrderItem(this,orderItem,deliveryQuantity,1,true));
        }

        public void setDeliveryOrderItem(DeliveryOrderItem deliveryOrderItem, decimal quantity)
        {
            GetDeliveryOrderItems(deliveryOrderItem.Id).SetBalanceQuantity(quantity);
        }

        public DeliveryOrderItem GetDeliveryOrderItems(int deliveryOrderItemId)
        {
            return _deliveryOrderItem.FirstOrDefault(invItem => invItem.Id == deliveryOrderItemId);
        }
    }
}
