
using AenEnterprise.DataAccess;
using AenEnterprise.DataAccess.Repository;
using AenEnterprise.DataAccess.RepositoryInterface;
using AenEnterprise.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  AenEnterprise.DataAccess.Repository
{
    public class ProductRepository:GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(AenEnterpriseDbContext context):base(context)
        {
            
        }
    }
}
