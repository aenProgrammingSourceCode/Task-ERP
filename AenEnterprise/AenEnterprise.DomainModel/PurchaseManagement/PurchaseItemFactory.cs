using AenEnterprise.DomainModel.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.PurchaseManagement
{
    public class PurchaseItemFactory
    {
        public static PurchaseItem CreatePurchaseItemFactory(PurchaseOrder purchaseOrder,Product product,
            decimal quantity,Unit unit,decimal price, decimal discountAmount,decimal discountPercent)
        {
            return new PurchaseItem(purchaseOrder, product,
            quantity,unit, price, discountAmount, discountPercent);
        }
    }
}
