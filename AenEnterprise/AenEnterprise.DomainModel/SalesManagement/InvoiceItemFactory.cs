using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class InvoiceItemFactory
    {
        public static InvoiceItem CreateInvoiceItemFactory(Invoice invoice, OrderItem orderItem, decimal invoiceQuantity, int statusId, bool isActive)
        {
            return new InvoiceItem(invoice, orderItem,invoiceQuantity, statusId, isActive);
        }
    }
}
