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

namespace OnlineShop.Api.Controllers
{
    public class UserController : BaseController
    {
        [Dependency]
        private IUnitOfWork UnitOfWork { get; }

        public UserController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }
        
        [Route("api/user_debit_balance")]
        public async Task<HttpResponseMessage> DebitUserBalance(PaymentModel payment)
        {
            var user = await UnitOfWork.Users.GetUser(payment.UserId);
            var balanceAfter = user.Balance - payment.Amount;

            if (balanceAfter < 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Payment invalid: balance < amount");
            }
            user.Balance = balanceAfter;
            await UnitOfWork.Users.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/user_charge_balance")]
        public async Task<HttpResponseMessage> ChargeUserBalance(PaymentModel payment)
        {
            (await UnitOfWork.Users.GetUser(payment.UserId)).Balance += payment.Amount;
            await UnitOfWork.Users.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }
        
        [Route("api/user_info/{userId:int}")]
        public async Task<HttpResponseMessage> GetUserData(int userId)
        {
            var obj = await UnitOfWork.Users.GetUser(userId);
            Mapper.Initialize(expression => expression.CreateMap(typeof(User), typeof(UserModel)));
            var model = Mapper.Map<User, UserModel>(obj);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }

//        [Route("api/user_auth")]
//        public async Task<HttpResponseMessage> AuthenticateUser(AuthenticationModel model)
//        {
//            
//        }
    }
}
