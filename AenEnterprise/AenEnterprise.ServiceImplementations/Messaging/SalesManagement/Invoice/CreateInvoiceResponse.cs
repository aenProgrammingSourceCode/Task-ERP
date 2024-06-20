using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice
{
    public class CreateInvoiceResponse
    {
        public InvoiceView Invoice { get; set; }
        public string ErrorMessage { get; set; }
    }
}
