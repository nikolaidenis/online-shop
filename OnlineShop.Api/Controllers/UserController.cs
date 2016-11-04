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
    public class UserController : ApiController
    {
        public readonly IUserRepository Repository;

        public UserController()
        {
//            Repository = new UserRepository();
        }

        [Route("api/user_debit_balance")]
        public async Task<HttpResponseMessage> DebitUserBalance(PaymentModel payment)
        {
            var user = await Repository.GetUser(payment.UserId);
            var balanceAfter = user.Balance - payment.Amount;

            if (balanceAfter < 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Payment invalid: balance < amount");
            }
            user.Balance = balanceAfter;
            await Repository.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/user_charge_balance")]
        public async Task<HttpResponseMessage> ChargeUserBalance(PaymentModel payment)
        {
            (await Repository.GetUser(payment.UserId)).Balance += payment.Amount;
            await Repository.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/user_info/{userId:int}")]
        public async Task<HttpResponseMessage> GetUserData(int userId)
        {
            var obj = await Repository.GetUser(userId);
            Mapper.Initialize(expression => expression.CreateMap(typeof(User), typeof(UserModel)));
            var model = Mapper.Map<User, UserModel>(obj);
            return Request.CreateResponse(HttpStatusCode.OK, model);
        }
    }
}
