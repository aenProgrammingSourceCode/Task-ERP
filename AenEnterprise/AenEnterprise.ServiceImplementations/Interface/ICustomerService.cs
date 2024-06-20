using AenEnterprise.ServiceImplementations.Messaging.CustomerManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Interface
{
    public interface ICustomerService
    {
       Task<GetAllCustomerResponse> GetAllCustomer();

    }
}
