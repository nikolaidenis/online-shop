using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using OnlineShop.Core;
using OnlineShop.Core.Data;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Web.Controllers
{
    public class ProductsController : ApiController
    {
        private readonly IProductRepository repository;

        public ProductsController()
        {
            repository = new ProductRepository();
        }

        public async Task<HttpResponseMessage> Get()
        {
            var products = await repository.GetProducts();
            var resultList = products as IList<Product> ?? products.ToList();
            return !resultList.Any() 
                ? Request.CreateErrorResponse(HttpStatusCode.NotFound, "Products not found!") 
                : Request.CreateResponse(HttpStatusCode.OK, resultList);
        }
    }
}
