using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel
{
    public class InvoiceItemView
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal InvoiceQuantity { get; set; }
        public decimal InvoiceAmount { get; set; }
        public int ApprovalId { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public string UnitName { get; set; } = string.Empty;
        public string ApprovalStatus { get; set; } = string.Empty;
        public decimal BalanceQuantity { get; set; }
        public decimal BalanceAmount { get; set; }
        public decimal DeliveryQuantity { get; set; }
        public decimal DeliveryAmount { get; set; }
        public int ProductId { get; set; }
        public int UnitId { get; set; }
        public int InvoiceId { get; set; }
        public int OrderItemId { get; set; }
    }
}
