using AenEnterprise.DataAccess.Repository;
using AenEnterprise.DomainModel.PurchaseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.RepositoryInterface
{
    public interface IPurchaseOrderRepository:IGenericRepository<PurchaseOrder>
    {
    }
}
