using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class CustomerRepository: GenericRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AenEnterpriseDbContext context) : base(context)
        {
            
        }
    }
}
