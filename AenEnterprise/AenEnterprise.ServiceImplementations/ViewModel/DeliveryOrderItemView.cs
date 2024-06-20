using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel
{
    public class DeliveryOrderItemView
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal DeliveryQuantity { get; set; }
        public decimal BalanceQuantity { get; set; }
        public decimal DeliveryAmount { get; set; }
        public int DeliveryOrderId { get; set; }
        public int OrderItemId { get; set; }
        public int ApprovalId { get; set; }
        public int StatusId { get; set; }
        public bool IsActive { get; set; }

    }
}
