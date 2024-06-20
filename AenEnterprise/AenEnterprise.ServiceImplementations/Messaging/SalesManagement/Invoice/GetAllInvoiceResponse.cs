using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.SalesManagement;
using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice
{
    public class GetAllInvoiceResponse
    {
        public IEnumerable<InvoiceView> Invoices { get; set; }
        public InvoiceView Invoice { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
    }
}
