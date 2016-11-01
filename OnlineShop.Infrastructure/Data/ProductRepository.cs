using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class ProductRepository: IProductRepository
    {
        private readonly UnitOfWork unitOfWork;
        public ProductRepository()
        {
            unitOfWork = new UnitOfWork();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await unitOfWork.Products.GetAll();
        }

        public Task<Product> GetProduct(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
