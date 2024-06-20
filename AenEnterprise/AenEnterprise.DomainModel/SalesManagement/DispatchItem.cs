using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class DispatchItem
    {
        private decimal _dispatchQuantity;
        private DispatcheOrder _dispatchOrder;
        public DispatchItem()
        {
        }
        public DispatchItem(DispatcheOrder dispatcheOrder, OrderItem orderItem,
          string vehicalNo, decimal vehicalEmptyWeight, decimal vehicalLoadWeight, decimal quantity, int statusId, bool isActive)
        {
            CreatedDate = DateTime.Today;
            VehicalNo = vehicalNo;
            VehicalEmptyWeight = vehicalEmptyWeight;
            VehicalLoadWeight = vehicalLoadWeight;
            OrderItem = orderItem;
            _dispatchOrder = dispatcheOrder;
            _dispatchQuantity = quantity;
            StatusId = statusId;
            IsActive = isActive; // Default value for bool
            IsPartiallyApproved = false; // Default value for bool
            IsFullyApproved = false; // Default value for bool
            DispatchAmount = quantity * orderItem.Price;
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string VehicalNo { get; set; } = string.Empty;
        public decimal VehicalEmptyWeight { get; set; }
        public decimal VehicalLoadWeight { get; set; }
        public int OrderItemId { get; set; }
        public OrderItem OrderItem { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool IsActive { get; set; }
        public decimal DispatchQuantity { get => _dispatchQuantity; set => _dispatchQuantity = value; }
        public bool IsPartiallyApproved { get; set; }
        public bool IsFullyApproved { get; set; }
        public int DispatchOrderId { get; set; }
        public DispatcheOrder DispatchOrder { get => _dispatchOrder; set => _dispatchOrder = value; }
        public decimal DispatchAmount { get; set; }
    }
}
