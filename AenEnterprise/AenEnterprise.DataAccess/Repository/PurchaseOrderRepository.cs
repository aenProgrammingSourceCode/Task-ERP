using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel.PurchaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class PurchaseOrderRepository : GenericRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(AenEnterpriseDbContext context) : base(context)
        {
        }
    }
}
 