using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using OnlineShop.Core.Data;
using Microsoft.Practices.Unity;
using System.Threading.Tasks;
using AutoMapper;
using OnlineShop.Api.Models;
using OnlineShop.Core;

namespace OnlineShop.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        [Dependency]
        public IUnitOfWork UnitOfWork { get; }

        public AdminController(IUnitOfWork uof)
        {
            UnitOfWork = uof;
        }

        [HttpGet]
        [Route("api/get_users")]
        public async Task<HttpResponseMessage> GetUsers()
        {
            var users = await UnitOfWork.Users.GetUsers();
            Mapper.Initialize(expr => expr.CreateMap(typeof(User), typeof(AccountModel))
                .ForMember("Username",options=>options.MapFrom("UserName"))
                .ForMember("Blocked", options=>options.MapFrom("IsBlocked"))
                .ForMember("EmailConfirmed", options=>options.MapFrom("IsEmailConfirmed")));
            var accounts = users.Select(Mapper.Map<User, AccountModel>).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, accounts);
        }

        [HttpPost]
        [Route("api/block_user")]
        public async Task<HttpResponseMessage> ChangeUserLocking(UserLockModel model)
        {
            var user = await UnitOfWork.Users.GetUser(model.Username);
            if (user.IsBlocked != model.IsBlocking)
            {
                user.IsBlocked = model.IsBlocking;
                await UnitOfWork.Users.UpdateUser(user);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            return Request.CreateErrorResponse(HttpStatusCode.Conflict,"user is already "+ (model.IsBlocking ? "blocked":"unlocked"));
        }
    }
}
