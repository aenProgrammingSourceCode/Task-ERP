using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging
{
    public class UpdateProductQuantityRequest
    {
        public int ProductId { get; set; }
        public decimal NewQuantity { get; set; }
    }
}
