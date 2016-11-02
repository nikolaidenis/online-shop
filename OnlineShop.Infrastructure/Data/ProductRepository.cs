using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class ProductRepository: IProductRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public ProductRepository()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await _unitOfWork.Products.GetAll();
        }

        public Task<Product> GetProduct(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
