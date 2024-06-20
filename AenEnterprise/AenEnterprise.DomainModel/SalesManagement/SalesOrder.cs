using Microsoft.CodeAnalysis.CSharp.Syntax; 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;
using System;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class SalesOrder
    {
        public int Id { get; set; }
        private IList<OrderItem> _orderItems;
        private IList<Invoice> _invoices;
        private IEnumerable<DeliveryOrder> _deliveryOrders;
        private IEnumerable<DispatcheOrder> _dispatcheOrders;
        
        public SalesOrder()
        {
            _orderItems = new List<OrderItem>();
            _invoices = new List<Invoice>();
            _deliveryOrders=new List<DeliveryOrder>();
            _dispatcheOrders=new List<DispatcheOrder>();
        }
        public string DeliveryPlane { get; set; }
        public string SalesOrderNo { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
        public DateTime OrderedDate { get; set; }
        public int SalesOrderStatusId { get; set; }
        public SalesOrderStatus SalesOrderStatus { get; set; }
        
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public IEnumerable<OrderItem> OrderItems { get { return _orderItems; } }
        public IEnumerable<Invoice> Invoices { get { return _invoices; } }
        public IEnumerable<DeliveryOrder> DeliveryOrders { get => _deliveryOrders; set => _deliveryOrders = value; }
        public IEnumerable<DispatcheOrder> DispatcheOrders { get => _dispatcheOrders; set => _dispatcheOrders = value; }
        public void CreateOrderItem(Product product, Unit unit, decimal quantity, decimal price, decimal? discountPercent,decimal invoiceQuantity,int statusId, bool isActive)
        {
            _orderItems.Add(OrderItemFactory.CreateOrderItemFactory(product, this, unit, quantity, price, discountPercent,invoiceQuantity,statusId,isActive));
        }
        public void UpdateOrderItem(Product product, SalesOrder salesOrder, Unit unit, decimal quantity, decimal price, decimal discountPercent, decimal invoiceQuantity,int statusId,bool isActive)
        {
            _orderItems.Add(OrderItemFactory.CreateOrderItemFactory(product, salesOrder, unit, quantity, price, discountPercent, invoiceQuantity, statusId, isActive));
        }
        

        public void setOrderItem(OrderItem orderItem, decimal quantity)
        {
            GetorderItems(orderItem.Id).SetBalanceQuantity(quantity);
        }

        public OrderItem GetorderItems(int orderItemId)
        {
            return _orderItems.FirstOrDefault(orderItem => orderItem.Id == orderItemId);
        }

        public OrderItem LastOrderItem(int orderItemId)
        {
            return _orderItems.Last(item => item.Id == orderItemId);
        }

    }

}
