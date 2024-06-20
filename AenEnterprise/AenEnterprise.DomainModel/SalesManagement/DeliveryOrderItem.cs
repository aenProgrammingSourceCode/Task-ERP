using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class DeliveryOrderItem
    {
        private decimal _balanceQuantity;
        private decimal _deliveryQuantity;
        private decimal _deliveryAmount;
        private decimal _balanceAmount;
        private decimal _dispatchQuantity;
        private decimal _dispatchAmount;
        private OrderItem _orderItem;
        public DeliveryOrderItem()
        {
        }
        public DeliveryOrderItem(DeliveryOrder deliveryOrder,OrderItem orderItem, 
            decimal deliveryQuantity, 
            int statusId, 
            bool isActive)
        {
            CreatedDate = DateTime.Today;
            DeliveryOrder = deliveryOrder;
            OrderItem = orderItem;
            _dispatchQuantity = 0.00m;
            _dispatchAmount = 0.00m;
            _deliveryQuantity = deliveryQuantity;
            _deliveryAmount = 0.00m; 
            IsPartiallyApproved = false; 
            IsFullyApproved = false; 
            StatusId = statusId;
            IsActive = isActive;
            _balanceQuantity = deliveryQuantity;
            _balanceAmount = 0.00m;
            
        }

        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DeliveryOrder DeliveryOrder { get; set; }
        public int DeliveryOrderId { get; set; }
        
        public bool IsPartiallyApproved { get; set; }
        public bool IsFullyApproved { get; set; }
        public Status Status { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }
        public decimal BalanceQuantity { get => _balanceQuantity; set => _balanceQuantity = value; }
        public decimal DeliveryQuantity { get => _deliveryQuantity; set => _deliveryQuantity = value; }
        public decimal DeliveryAmount { get => _deliveryAmount; set => _deliveryAmount = value; }
        public decimal BalanceAmount { get => _balanceAmount; set => _balanceAmount = value; }
        public decimal DispatchQuantity { get => _dispatchQuantity; set => _dispatchQuantity = value; }
        public decimal DispatchAmount { get => _dispatchAmount; set => _dispatchAmount = value; }
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get => _orderItem; set => _orderItem = value; }

        public void SetBalanceQuantity(decimal quantity)
        {
            BalanceQuantity -= quantity;
            DeliveryQuantity += quantity;

            if (BalanceQuantity == 0)
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
