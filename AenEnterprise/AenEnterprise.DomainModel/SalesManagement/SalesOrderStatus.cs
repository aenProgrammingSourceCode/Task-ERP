using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DomainModel.SalesManagement
{
    public class SalesOrderStatus
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<SalesOrder> SalesOrders { get; set; }

    }
}
