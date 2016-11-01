using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly DbContext context;
        private readonly DbSet<T> dbSet;

        public Repository():this(new ShopContext()) { } 

        public Repository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        } 

        public async Task<T> Create(T model)
        {
            return dbSet.Add(model);
        }

        public async Task<T> Get(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task Delete(T obj)
        {
            context.Entry(obj).State = EntityState.Deleted;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dbSet.AsQueryable().ToListAsync();
        }
    }
}
