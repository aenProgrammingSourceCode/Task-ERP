using AenEnterprise.DataAccess;
using AenEnterprise.DataAccess.Repository;
using AenEnterprise.DataAccess.RepositoryInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AenEnterpriseDbContext _context;

        public UnitOfWork(AenEnterpriseDbContext context)
        {
            _context = context;
            Category = new CategoryRepository(_context);
            Product = new ProductRepository(_context);
        }

        public ICategoryRepository Category { get; set; }
        public IProductRepository Product { get; set; }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
