using AenEnterprise.DomainModel.PurchaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.InventoryManagement
{
    public class StockItem
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public int PurchaseItemId { get; set; }
        public PurchaseItem PurchaseItem { get; set; }
        public decimal CurrentStockQuantity { get; set; }
        public decimal NewStockQuantity { get; set; }
    }
}
