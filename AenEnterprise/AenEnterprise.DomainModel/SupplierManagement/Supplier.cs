using AenEnterprise.DomainModel.PurchaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SupplierManagement
{
    public class Supplier
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public bool IsCompany { get; set; }
        public string Description { get; set; }
        public List<PurchaseOrder> PurchaseOrders { get; set; }
    }
}
