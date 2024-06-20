using AenEnterprise.DomainModel.Inventory;
using AenEnterprise.DomainModel.PurchaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.InventoryManagement
{
    public class Stock
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ReceivedDate { get; set; }
        public int PurchaseOrderId { get; set; }
        public PurchaseOrder PurchaseOrder { get; set; }
        public int PurchaseId { get; set; }
        public Warehouse Warehouse { get; set; }
        public int WareHouseId { get; set; }
    }
}
