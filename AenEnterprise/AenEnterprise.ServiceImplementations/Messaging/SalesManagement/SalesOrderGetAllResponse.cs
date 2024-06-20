using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.SalesManagement
{
    public class SalesOrderGetAllResponse
    {
        public IEnumerable<SalesOrderView> SalesOrderViews { get; set; }
        public SalesOrderView SalesOrderView { get; set; }
    }
}
