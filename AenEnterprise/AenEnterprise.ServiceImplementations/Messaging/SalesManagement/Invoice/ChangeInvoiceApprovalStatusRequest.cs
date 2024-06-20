using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.Invoice
{
    public class ChangeInvoiceApprovalStatusRequest
    {
        public int InvoiceId { get; set; }
        public int ApprovalId { get; set; }
    }
}
