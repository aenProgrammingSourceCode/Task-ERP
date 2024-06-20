using AenEnterprise.DomainModel.SupplierManagement;
using AenEnterprise.ServiceImplementations.ViewModel.InventoryVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.InventoryManagement
{
    public class CreatePurchaseOrderRequest
    {
        public int Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseOrderNo { get; set; }
        public int PurchaseOrderId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
    }
}
