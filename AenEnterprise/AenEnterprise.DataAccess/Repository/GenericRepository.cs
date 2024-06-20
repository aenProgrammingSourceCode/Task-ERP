
using AenEnterprise.DataAccess;
using AenEnterprise.DataAccess.RepositoryInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AenEnterprise.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AenEnterpriseDbContext _context;

        public GenericRepository(AenEnterpriseDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _context.Set<T>().AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> FindAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task RemoveAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> SearchAsync(Func<T, bool> predicate)
        {
            return await _context.Set<T>().Where(predicate).AsQueryable().ToListAsync();
        }


        //using Microsoft.EntityFrameworkCore;
        //using System;
        //using System.Linq;
        //using System.Threading.Tasks;
        //using System.Linq.Expressions;

        //public interface IRepository<TEntity> where TEntity : class
        //    {
        //        TEntity GetById(int id);
        //        Task<TEntity> GetByIdAsync(int id);
        //        IQueryable<TEntity> GetAll();
        //        IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        //        void Add(TEntity entity);
        //        void Update(TEntity entity);
        //        void Delete(TEntity entity);
        //    }

        //public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
        //{
        //    private readonly DbContext _context;
        //    private readonly DbSet<TEntity> _dbSet;

        //    public Repository(DbContext context)
        //    {
        //        _context = context ?? throw new ArgumentNullException(nameof(context));
        //        _dbSet = _context.Set<TEntity>();
        //    }

        //    public TEntity GetById(int id)
        //    {
        //        return _dbSet.Find(id);
        //    }

        //    public async Task<TEntity> GetByIdAsync(int id)
        //    {
        //        return await _dbSet.FindAsync(id);
        //    }

        //    public IQueryable<TEntity> GetAll()
        //    {
        //        return _dbSet;
        //    }

        //    public IQueryable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        //    {
        //        return _dbSet.Where(predicate);
        //    }

        //    public void Add(TEntity entity)
        //    {
        //        _dbSet.Add(entity);
        //    }

        //    public void Update(TEntity entity)
        //    {
        //        _dbSet.Attach(entity);
        //        _context.Entry(entity).State = EntityState.Modified;
        //    }

        //    public void Delete(TEntity entity)
        //    {
        //        _dbSet.Remove(entity);
        //    }
        //}


    }
}
