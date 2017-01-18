using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OnlineShop.Api.Helpers;
using OnlineShop.Api.Models;

namespace OnlineShop.Api.Controllers
{
    [Authorize]
    public class BaseController : ApiController
    {
    }
}
