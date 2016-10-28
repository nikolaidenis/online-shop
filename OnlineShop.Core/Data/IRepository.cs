using System.Linq;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IRepository<T> where T:class
    {
        Task<IQueryable<T>> GetAll();
        Task Create(T model);
        Task Update(T model);
        Task Delete(T id);
        Task<T> GetById(int id);
    }
}
