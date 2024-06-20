using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class OrderItem
    {
        private int _productId;
        private Unit _unit;
        private Product _product;
        private SalesOrder _salesOrder;
        private List<DispatchItem> _dispatchItems;
        private decimal _price;
        private decimal _quantity;
        private decimal _amount;
        private decimal _balanceQuantity;
        private decimal _invoicedQuantity;
        private decimal? _invoicedAmount;
        private string _orderItemNo;
        private decimal? _discountPercent;
        private decimal _netAmount;
        public OrderItem()
        {

        }
        public OrderItem(Product product, SalesOrder order, Unit unit, decimal quantity, 
           decimal price,decimal? discountPercent,decimal invoicedQuantity,int statusId,bool isActive)
        {
            _unit = unit;
            _salesOrder = order;
            _product = product;
            _price = price;
            _quantity = quantity;
            _balanceQuantity = quantity;
            _invoicedQuantity = 0.00m;
            _amount = price * quantity;
            DiscountPercent = discountPercent;
            _netAmount = (price * quantity) - ((price * quantity) * (discountPercent?? 0m) / 100);
            StatusId = statusId;
            IsActive = isActive;
            _orderItemNo = "Item-N";
        }
        public bool IsPartiallyApproved { get; set; }
        public bool IsFullyApproved { get; set; }
        public int Id { get; set; }
        public int SalesOrderId { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get => _price; set => _price = value; }
        public Product Product { get => _product; set => _product = value; }
        public decimal Quantity { get => _quantity; set => _quantity = value; }
        public SalesOrder SalesOrder { get => _salesOrder; set => _salesOrder = value; }
        public Unit Unit { get => _unit; set => _unit = value; }
        public decimal Amount { get => _amount; set => _amount = value; }
        public decimal BalanceQuantity { get => _balanceQuantity; set => _balanceQuantity = value; }
        public decimal InvoicedQuantity { get => _invoicedQuantity; set => _invoicedQuantity = value; }
        public int ProductId { get => _productId; set => _productId = value; }
        public decimal NetAmount { get => _netAmount; set => _netAmount = value; }
        public decimal? DiscountPercent { get => _discountPercent; set => _discountPercent = value; }
        public decimal InvoicedAmount { get => Price*InvoicedQuantity; set => _invoicedAmount = value; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool IsActive { get; set; }
        public string OrderItemNo { get => _orderItemNo; set => _orderItemNo = value; }
        private List<DeliveryOrderItem> _deliveryOrderItems;
        public List<DeliveryOrderItem> DeliveryOrderItems { get => _deliveryOrderItems; set => _deliveryOrderItems = value; }
        public List<DispatchItem> DispatchItems { get => _dispatchItems; set => _dispatchItems = value; }

        public void SetBalanceQuantity(decimal quantity)
        {
            _balanceQuantity -= quantity;
            _invoicedQuantity += quantity;

            if (_balanceQuantity==0)
            {
                StatusId = 3;
                IsPartiallyApproved = false;
                IsFullyApproved = true;
            }
            else
            {
               
                StatusId = 2;
                IsPartiallyApproved = true;
                IsFullyApproved = false;
            }
            
        }
    }
}
