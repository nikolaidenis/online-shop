using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.Practices.Unity;
using OnlineShop.Api.Filters;
using OnlineShop.Api.Models;
using OnlineShop.Core;
using OnlineShop.Core.Data;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Api.Controllers
{
    public class ProductController : BaseController
    {
        [Dependency]
        private IUnitOfWork UnitOfWork { get; }
        public ProductController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        
        public async Task<HttpResponseMessage> Get()
        {
            var products = await UnitOfWork.Products.GetProducts();
            Mapper.Initialize(expression => expression.CreateMap(typeof(Product), typeof(ProductModel)));
            var productsModel = Mapper.Map<IEnumerable<Product>,List<ProductModel>>(products);

            return Request.CreateResponse(HttpStatusCode.OK, productsModel);
        }
    }
}
