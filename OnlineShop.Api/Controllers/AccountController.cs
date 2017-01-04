using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Security.Claims;
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
            if (user == 0)
            {
                return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
            var token = GenerateToken();
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new StringContent(token);

            return response;
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
            try
            {
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
                
                using (var messenger = new EmailService())
                {
                    await messenger.CreateConfirmationEmail(GenerateConfirmationToken(userObj.Id), userObj.Email);
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, userObj);
            }
            catch(SqlException e)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Conflict,
                    $"{"Cannot insert user into database, message: "}:{e.InnerException}");
            }
        }

        
        [Route("api/confirmation/{code}")]
        [System.Web.Http.HttpGet]
        public async Task<HttpResponseMessage> ConfirmEmail(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Token is empty");
            }

            int userId = ParseConfirmationToken(code);
            var user = await UnitOfWork.Users.GetUser(userId);
            if (user.IsEmailConfirmed)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotAcceptable, "User confirmed");
            }
            user?.ConfirmEmail();
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

        private string GenerateConfirmationToken(int id)
        {
            string tokenStr = string.Empty;
            var bytes = BitConverter.GetBytes(id);
            tokenStr = bytes.Aggregate(tokenStr, (current, b) => current + b);
            return tokenStr;
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
