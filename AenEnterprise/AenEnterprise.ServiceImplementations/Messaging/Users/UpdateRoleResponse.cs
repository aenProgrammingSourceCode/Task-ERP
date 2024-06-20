using AenEnterprise.DomainModel.UserDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.ServiceImplementations.Messaging.Users
{
    public class UpdateRoleResponse
    {
        public IEnumerable<Role> Roles { get; set; }
    }
}
