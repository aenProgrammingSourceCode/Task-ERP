using AenEnterprise.DomainModel.InventoryManagement;
using AenEnterprise.DomainModel.PurchaseManagement;
using AenEnterprise.DomainModel.SupplierManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel.InventoryVM
{
    public class PurchaseOrderView
    {
        public PurchaseOrderView()
        {
            PurchaseItems = new List<PurchaseItemView>();
        }
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string PurchaseOrderNo { get; set; }
        public Supplier Supplier { get; set; }
        public int SupplierId { get; set; }
        public List<PurchaseItemView> PurchaseItems { get; set; }
    }
}
