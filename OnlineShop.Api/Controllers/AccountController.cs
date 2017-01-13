using System;
using System.Web.Http;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Practices.Unity;
using OnlineShop.Api.Helpers;
using OnlineShop.Api.Models;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Api.Controllers
{
    public class AccountController : BaseController
    {
        [Dependency]
        private IUnitOfWork UnitOfWork { get; }

        public AccountController(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        [AllowAnonymous]
        [Route("api/account/login")]
        public async Task<HttpResponseMessage> Login(AuthenticationModel model)
        {
            var user = await UnitOfWork.Users.AuthenticateUser(model.Username, model.Password);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "wrong login or password");
            }

            string tokenStr = GenerateToken();
            await UnitOfWork.UserSessions.CreateNewSession(user.Id, tokenStr);

            var returnObj = new {token = tokenStr, username = model.Username};

            return Request.CreateResponse(HttpStatusCode.OK, returnObj);
        }

        [Route("api/account")]
        [HttpGet]
        public async Task<HttpResponseMessage> GetUserBySession([FromUri]string session)
        {
            if (string.IsNullOrEmpty(session))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "token cannot be empty");
            }
            var userId = await UnitOfWork.UserSessions.GetUserByValidSession(session);
            
            return userId != 0 ? Request.CreateResponse(HttpStatusCode.OK,userId) : new HttpResponseMessage(HttpStatusCode.Unauthorized);
        }

        [Route("api/account/logout")]
        [HttpGet]
        public async Task<HttpResponseMessage> Logout([FromUri]string session)
        {
            if (string.IsNullOrEmpty(session))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "token cannot be empty");
            }

            await UnitOfWork.UserSessions.SetSessionExpired(session);

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        [Route("api/account/{userId:int}/{token}")]
        [HttpGet]
        public async Task<HttpResponseMessage> CheckUserToken(int userId, string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "token cannot be empty");
            }
            var id = await UnitOfWork.UserSessions.GetUserByValidSession(token);
            return id == userId ? Request.CreateResponse(HttpStatusCode.OK) 
                : Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "invalid session for current user");
        }

        [Route("api/account/signup")]
        public async Task<HttpResponseMessage> Signup(SignupModel model)
        {
            if (!model.IsValid())
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict,
                    "Model is not valid!");
            }
            if (await UnitOfWork.Users.IsUserExist(model.Username))
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict,
                    "User with such nickname already exist!");
            }

            var userObj = new User
            {
                UserName = model.Username,
                Password = model.Password,
                RoleId = 2,
                IsBlocked = false,
                IsEmailConfirmed = false,
                Email = model.Email
            };
            userObj.Id = await UnitOfWork.Users.CreateUser(userObj);
            userObj.ActivationCode = GenerateConfirmationToken(userObj.Id,userObj.UserName);
            await UnitOfWork.Users.UpdateUser(userObj);

            var callbackUrl = $"http://localhost:3315/api/confirmation?id={userObj.Id}&code={userObj.ActivationCode}";

            using (var messenger = new EmailService())
            {
                await messenger.CreateConfirmationEmail(callbackUrl, userObj.Email);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        
        [Route("api/confirmation")]
        [HttpGet]
        public async Task<HttpResponseMessage> ConfirmEmail(string id, string code)
        {
            var userId = Convert.ToInt32(id);
            if (string.IsNullOrEmpty(code) || userId == 0)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Wrong parameters");
            }
            
            var user = await UnitOfWork.Users.GetUser(userId);
            if (user == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "User with id:"+id+" is not exist");
            }
            if (user.IsEmailConfirmed)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "User already confirmed");
            }
            if (user.ActivationCode != code)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict, "Wrong token");
            }

            user.IsEmailConfirmed = true;
            await UnitOfWork.Users.UpdateUser(user);
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //toDo: move to extensions; encrypt date
        private string GenerateToken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            return HttpUtility.UrlEncode(token);
        }

        private string GenerateConfirmationToken(int id, string name)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(id + name));
                return new Guid(hash).ToString().Replace("-","");
            }
        }

        private bool IsOldToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            var when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            return when < DateTime.UtcNow.AddHours(-24);
        }
    }
}
