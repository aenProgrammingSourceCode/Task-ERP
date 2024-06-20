using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice
{
    public class CreateInvoiceRequest
    {
        public CreateInvoiceRequest()
        {
            orderItemIdToAdd = new List<int>();
        }
        public IList<int> orderItemIdToAdd { get; set; }

        public int InvoiceId { get; set; }
        public int InvoiceItemId { get; set; }
        public decimal InvoiceQuantity { get; set; }
        //public decimal InvoiceAmount { get; set; }
        public int SalesOrderId { get; set; }
        //public int ProductId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        //public decimal Price { get; set; }
        //public int UnitId { get; set; }
        public int OrderItemId { get; set; }
    }
}
