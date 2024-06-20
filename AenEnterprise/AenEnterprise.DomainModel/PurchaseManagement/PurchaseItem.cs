using AenEnterprise.DomainModel.InventoryManagement;
using AenEnterprise.DomainModel.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.PurchaseManagement
{
    public class PurchaseItem
    {
        public PurchaseItem()
        {

        }
        public PurchaseItem(PurchaseOrder purchaseOrder, Product product,
            decimal quantity, Unit unit, decimal price, decimal discountAmount, decimal discountPercent)
        {
            CreatedDate = DateTime.Today;
            PurchaseOrder = purchaseOrder;
            Product = product;
            Quantity = quantity;
            Price = price;
            DiscountAmount = discountAmount;
            DiscountPercent = discountPercent;
            
        }

        public DateTime CreatedDate { get; set; }
        public int Id { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public int PurchaseOrderId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public Unit Unit { get; set; }
        public int UnitId { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal TotalCost { get; set; }
        public List<StockItem> StockItems { get; set; }
    }
}
