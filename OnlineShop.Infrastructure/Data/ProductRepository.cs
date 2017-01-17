using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;


namespace OnlineShop.Infrastructure.Data
{
    public class ProductRepository: Repository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context):base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await GetAll();
        }
        public Task<Product> GetProduct(int id)
        {
            throw new System.NotImplementedException();
        }
    }
}
