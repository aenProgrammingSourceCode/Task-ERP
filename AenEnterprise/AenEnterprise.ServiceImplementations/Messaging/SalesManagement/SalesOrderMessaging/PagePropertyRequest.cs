using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging
{
    public class PagePropertyRequest
    {
        public int currentPage { get; set; }
        public int totalPages { get; set; }
        public int maxPagesToShow { get; set; }
    }
}
