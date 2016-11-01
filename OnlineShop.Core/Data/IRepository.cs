using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IRepository<T> where T:class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Create(T model);
        Task Delete(T id);
        Task<T> Get(int id);
    }
}
