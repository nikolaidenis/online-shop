using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using OnlineShop.Api.Models;
using OnlineShop.Core.Data;

namespace OnlineShop.Api.Controllers
{
    [AllowAnonymous]
    public class AccountController : ApiController
    {
        [Dependency]
        private IUnitOfWork UnitOfWork { get; }

        public AccountController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [Route("api/account/login")]
        public async Task<HttpResponseMessage> Login(AuthenticationModel model)
        {
            var user = await UnitOfWork.Users.AuthenticateUser(model.Username, model.Password);
            if (user != 0)
            {
                await GenerateToken(HttpContext.Current);
                return new HttpResponseMessage(HttpStatusCode.OK);
            }
            return new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        private async Task GenerateToken(HttpContext context)
        {

        }
    }
}
