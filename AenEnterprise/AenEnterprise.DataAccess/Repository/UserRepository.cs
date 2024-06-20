using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using AenEnterprise.DomainModel.UserDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class UserRepository:GenericRepository<User>, IUserRepository
    {
        public UserRepository(AenEnterpriseDbContext context) : base(context)
        {

        }

        
    }
}
