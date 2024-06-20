using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.CustomerManagment
{
    public class GetAllCustomerResponse
    {
        public IEnumerable<CustomerView> Customers { get; set; }
    }
}
