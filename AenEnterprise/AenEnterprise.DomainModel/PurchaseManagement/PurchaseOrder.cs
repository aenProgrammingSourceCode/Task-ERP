using AenEnterprise.DomainModel.InventoryManagement;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.DomainModel.SupplierManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.PurchaseManagement
{
    public class PurchaseOrder
    {
        private IList<PurchaseItem> _purchaseItems;
        public PurchaseOrder()
        {
            CreateDate = DateTime.Today;
            _purchaseItems = new List<PurchaseItem>();
        }
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseOrderNo { get; set; }
        public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }
        
        public List<Stock> Stocks { get; set; }
        public IEnumerable<PurchaseItem> PurchaseItems { get => _purchaseItems; }

        public void CreatePurchaseItem(PurchaseOrder purchaseOrder, Product product,
            decimal quantity, Unit unit, decimal price, decimal discountAmount, decimal discountPercent)
        {
            _purchaseItems.Add(PurchaseItemFactory.CreatePurchaseItemFactory(purchaseOrder, product,
            quantity, unit, price, discountAmount, discountPercent));
        }
    }
}  
