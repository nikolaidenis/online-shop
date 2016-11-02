using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using OnlineShop.Api.Models;
using OnlineShop.Core;
using OnlineShop.Core.Data;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Api.Controllers
{
    public class ProductController : ApiController
    {
        private readonly IProductRepository _repository;

        public ProductController()
        {
            _repository = new ProductRepository();
        }

        public async Task<HttpResponseMessage> Get()
        {
            var products = await _repository.GetProducts();
            Mapper.Initialize(expression => expression.CreateMap(typeof(Product), typeof(ProductModel)));
            var productsModel = Mapper.Map<IEnumerable<Product>,List<ProductModel>>(products);

            return Request.CreateResponse(HttpStatusCode.OK, productsModel);
        }
    }
}
