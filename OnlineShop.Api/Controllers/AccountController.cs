using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using OnlineShop.Api.Helpers;
using OnlineShop.Api.Models;
using OnlineShop.Api.Providers;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Api.Controllers
{
    [System.Web.Http.AllowAnonymous]
    public class AccountController : BaseController
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
            return user == 0 ? Request.CreateErrorResponse(HttpStatusCode.Conflict,"wrong login or password") 
                : Request.CreateResponse(HttpStatusCode.OK, GenerateToken());
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
        [System.Web.Http.HttpGet]
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
            return token;
        }

        private string GenerateConfirmationToken(int id, string name)
        {
            using (var md5 = MD5.Create())
            {
                var hash = md5.ComputeHash(Encoding.Default.GetBytes(id + name));
                return new Guid(hash).ToString().Replace("-","");
            }
        }

        private int ParseConfirmationToken(string token)
        {
            var bytes = new byte[token.Length];
            var charArr = token.ToCharArray();
            for (int i = 0; i < token.Length; i++)
            {
                bytes[i] = (byte) charArr[i];
                bytes[i] -= 48;
            }
            return Convert.ToInt32(bytes);
        }

        private bool IsOldToken(string token)
        {
            byte[] data = Convert.FromBase64String(token);
            var when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            return when < DateTime.UtcNow.AddHours(-24);
        }
    }
}
