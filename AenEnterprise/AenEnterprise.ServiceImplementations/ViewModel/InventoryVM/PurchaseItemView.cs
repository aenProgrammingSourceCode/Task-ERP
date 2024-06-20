using AenEnterprise.DomainModel.InventoryManagement;
using AenEnterprise.DomainModel.PurchaseManagement;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel.InventoryVM
{
    public class PurchaseItemView
    {
        public int Id { get; set; }
        public int PurchaseOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalCost { get; set; }
    }
}
