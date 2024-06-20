using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel.UserDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class RoleRepository: GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(AenEnterpriseDbContext context) : base(context)
        {

        }
    }
}
