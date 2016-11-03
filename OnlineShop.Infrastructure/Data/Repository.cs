using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly DbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository():this(new OnlineShopEntities()) { } 

        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        } 

        public async Task<T> Create(T model)
        {
            return _dbSet.Add(model);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression)
        {
            return await _dbSet.FirstOrDefaultAsync(expression);
        }
        
        public async Task Delete(T obj)
        {
            _context.Entry(obj).State = EntityState.Deleted;
        }

        public async Task Update(T obj)
        {
            _context.Entry(obj).State = EntityState.Modified;
        } 

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _dbSet.AsQueryable().ToListAsync();
        }
        public IQueryable<T> GetAllAsQueryable()
        {
            IQueryable<T> queryable = _dbSet;
            return queryable.AsQueryable();
        }
    }
}
