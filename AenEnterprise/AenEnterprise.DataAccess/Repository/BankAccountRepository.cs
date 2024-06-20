using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class BankAccountRepository: GenericRepository<BankAccount>, IBankAccountRepository
    {
        public BankAccountRepository(AenEnterpriseDbContext context) : base(context)
        {

        }
    }
}
