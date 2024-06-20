using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement.SalesOrderMessaging
{
    public class ModifyOrderItemRequest
    {
        public int SalesOrderId { get; set; }
        public ModifyOrderItemRequest()
        {
            UpdateQuantity = new List<UpdateProductQuantityRequest>();
            ProductToAdd = new List<int>();
        }
        public IList<int> ProductToAdd { get; set; }
        public IList<UpdateProductQuantityRequest> UpdateQuantity { get; set; }
    }
}
