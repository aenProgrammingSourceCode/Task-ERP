using AenEnterprise.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.ViewModel
{
   public class InvoiceView
    {
        public InvoiceView()
        {
            InvoiceItems = new List<InvoiceItemView>();
        }
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int SalesOrderId { get; set; }
        public string InvoiceNo { get; set; } = string.Empty;
        public string CustomerName { get; set; }
        public int CustomerId { get; set; }
        public List<InvoiceItemView> InvoiceItems { get; set; }
    }
}
