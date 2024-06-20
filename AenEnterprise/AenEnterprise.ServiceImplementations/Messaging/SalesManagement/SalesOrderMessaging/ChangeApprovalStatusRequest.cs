using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging
{
    public class ChangeApprovalStatusRequest
    {
        public int SalesOrderId { get; set; }
        public int ApprovalId { get; set; }
    }
}
