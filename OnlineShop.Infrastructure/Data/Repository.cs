using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class Repository<T> : IRepository<T> where T:class
    {
        private readonly DbContext context;
        private readonly DbSet<T> dbSet;

        public Repository(DbContext context)
        {
            this.context = context;
            dbSet = context.Set<T>();
        } 
        public void Create(T model)
        {
            dbSet.Add(model);
        }

        public T GetById(int id)
        {
            return dbSet.Find(id);
        }

        public void Delete(T obj)
        {
            context.Entry(obj).State = EntityState.Deleted;
        }

        public IQueryable<T> GetAll()
        {
            return dbSet;
        }

        public void Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
