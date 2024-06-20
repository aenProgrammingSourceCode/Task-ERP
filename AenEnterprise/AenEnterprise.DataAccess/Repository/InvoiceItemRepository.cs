using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class InvoiceItemRepository:GenericRepository<InvoiceItem>, IInvoiceItemRepository
    {
        public InvoiceItemRepository(AenEnterpriseDbContext context) : base(context)
        {
            
        }
    }
}
