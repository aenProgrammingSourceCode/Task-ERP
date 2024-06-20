using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel.PurchaseManagement;
using AenEnterprise.DomainModel.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class PurchaseItemRepository : GenericRepository<PurchaseItem>, IPurchaseItemRepository
    {
        public PurchaseItemRepository(AenEnterpriseDbContext context) : base(context)
        {
        }
    }
}
