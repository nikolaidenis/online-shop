using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using OnlineShop.Core.Data;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Api.Providers
{
    public class CustomAuthorizationProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            string clientId, clientSecret;

            if (context.TryGetBasicCredentials(out clientId, out clientSecret))
            {
                context.Validated();
            }
            else
            {
                context.SetError("invalid_client", "Client credentials could not be retrieved from the Authorization header");
                context.Rejected();
            }

        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            //            return base.GrantResourceOwnerCredentials(context);
            var userManager = context.OwinContext.GetUserManager<UserManager<IdentityUser>>();
            var user = await userManager.FindAsync(context.UserName, context.Password);

            if (user == null)
            {
                context.SetError("invalid_grant", "Invalid User Id or password'");
                context.Rejected();
                return;
            }

            var identity = await userManager.CreateIdentityAsync(
                                                user,
                                                DefaultAuthenticationTypes.ExternalBearer);
            context.Validated(identity);




//            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });
//            if (await UnitOfWork.Users.AuthenticateUser(context.UserName, context.Password) == 0)
//            {
//                context.SetError("invalid_grant", "The user name or password is incorrect.");
//                return;
//            }
//
//            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
//            identity.AddClaim(new Claim("sub", context.UserName));
//            identity.AddClaim(new Claim("role", "user"));
//
//            context.Validated(identity);
        }
    }
}