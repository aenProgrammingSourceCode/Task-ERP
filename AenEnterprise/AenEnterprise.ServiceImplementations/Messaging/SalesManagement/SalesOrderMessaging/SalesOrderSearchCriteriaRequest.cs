using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging
{
    public class SalesOrderSearchCriteriaRequest
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string CriteriaName { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public bool oiIsFullyApproved { get; set; }
        public bool invIsFullyApproved { get; set; }
        public bool deliIsFullyApproved { get; set; }
        public bool oiIsPartiallyApproved { get; set; }
        public bool invIsPartiallyApproved { get; set; }
        public bool deliIsPartiallyApproved { get; set; }
        public int statusId { get; set; }
        public bool isActive { get; set; }
    }
}
