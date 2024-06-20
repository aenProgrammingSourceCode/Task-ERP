using AenEnterprise.DomainModel.InventoryManagement;
using AenEnterprise.DomainModel.PurchaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }=string.Empty;
        public List<OrderItem> OrderItems { get; set; }
        public List<PurchaseItem> PurchaseItems { get; set; }
    }
}
