using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class InvoiceItem
    {
        public int Id { get; set; }
        private decimal _invoiceQuantity;
        private decimal _invoiceAmount;
        private decimal _balanceQuantity;
        private decimal _balanceAmount;
        private decimal _deliveryQuantity;
        private decimal _deliveryAmount;
        private OrderItem _orderItem;
        
        public InvoiceItem()
        {

        }
        public InvoiceItem(Invoice invoice, OrderItem orderItem,decimal invoiceQuantity, int statusId, bool isActive)
        {
            CreatedDate = DateTime.Now;
            InvoiceQuantity = invoiceQuantity;
            _deliveryQuantity = 0.00m;
            StatusId=statusId;
            IsActive=isActive;  
            OrderItem = orderItem;
            Invoice = invoice;
            InvoiceAmount= orderItem.Price * invoiceQuantity;
            BalanceAmount = orderItem.Price * invoiceQuantity;
            _balanceQuantity = invoiceQuantity;
        }
        public int OrderItemId { get; set; }
         
        public bool IsPartiallyApproved { get; set; }
        public bool IsFullyApproved { get; set; }
        
        public DateTime CreatedDate { get; set; }
        
        public string? Description { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public bool IsActive { get; set; }
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
        public OrderItem OrderItem { get => _orderItem; set => _orderItem = value; }

        public decimal BalanceQuantity { get => _balanceQuantity; set => _balanceQuantity = value; }
        
        public decimal InvoiceQuantity { get => _invoiceQuantity; set => _invoiceQuantity = value; }
        public decimal InvoiceAmount { get => _invoiceAmount; set => _invoiceAmount = value; }
        public decimal DeliveryQuantity { get => _deliveryQuantity; set => _deliveryQuantity = value; }
        public decimal BalanceAmount { get => _balanceAmount; set => _balanceAmount = value; }
        public decimal DeliveryAmount { get => _deliveryAmount; set => _deliveryAmount = value; }

        public void SetBalanceQuantity(decimal quantity)
        {
            _balanceQuantity -= quantity;
            _deliveryQuantity += quantity;

            if (_balanceQuantity == 0)
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
