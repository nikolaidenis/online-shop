using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IRepository<T> where T:class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Create(T model);
        Task Delete(T id);
        Task Update(T obj);
        Task<T> Get(Expression<Func<T, bool>> expression);
        IQueryable<T> GetAllAsQueryable();
        void Commit();
        Task CommitAsync();
    }
}
