using AenEnterprise.ServiceImplementations.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.Users
{
    public class GetUserResponse
    {
        public IEnumerable<UserView> Users { get; set; }
        public UserView User { get; set; }
    }
}
