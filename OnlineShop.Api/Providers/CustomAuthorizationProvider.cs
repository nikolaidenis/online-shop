using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using OnlineShop.Core.Data;

namespace OnlineShop.Api.Providers
{
    public class CustomAuthorizationProvider : OAuthAuthorizationServerProvider
    {

        [Dependency]
        public IUnitOfWork UnitOfWork { get; set; }

        public CustomAuthorizationProvider(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            //            return base.ValidateClientAuthentication(context);
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //            return base.GrantResourceOwnerCredentials(context);
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
            if (await UnitOfWork.Users.AuthenticateUser(context.UserName, context.Password) == 0)
            {
                context.SetError("invalid_grant", "The user name or password is incorrect.");
                return;
            }

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
        }
    }
}