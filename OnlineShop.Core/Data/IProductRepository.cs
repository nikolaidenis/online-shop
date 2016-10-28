using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetProducts();
        Task<Product> GetProduct(int id);
    }
}
