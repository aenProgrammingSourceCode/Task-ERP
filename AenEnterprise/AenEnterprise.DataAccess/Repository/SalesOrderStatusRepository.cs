using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class SalesOrderStatusRepository: GenericRepository<SalesOrderStatus>, ISalesOrderStatusRepository
    {
        public SalesOrderStatusRepository(AenEnterpriseDbContext context) : base(context)
        {
                
        }
    }
}
