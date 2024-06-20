using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement
{
    public class GetAllSalesOrderResponse
    {
        public IEnumerable<SalesOrderView> SalesOrders { get; set; }
        public SalesOrderView SalesOrder { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalSalesOrderQuantity { get; set; }
        public decimal TotalSalesOrderAmount { get; set; }
        public decimal TotalInvoiceQuantity { get; set; }
        public decimal TotalInvoiceAmount { get; set; }
        public decimal TotalDispatchQuantity { get; set; }
        public decimal TotalDispatchAmount { get; set; }
    }
}
