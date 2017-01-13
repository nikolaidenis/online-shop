using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using OnlineShop.Api.Models;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Api.Providers
{
    public class CustomAuthorizationProvider : OAuthAuthorizationServerProvider
    {
        [Dependency]
        private IUnitOfWork UnitOfWork { get; set; }

        public CustomAuthorizationProvider(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }

        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            try
            {
                //retrieve your user from database. ex:
                var user = await UnitOfWork.Users.AuthenticateUser(context.UserName, context.Password);

                var identity = new ClaimsIdentity(context.Options.AuthenticationType);

                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

                var roles = new List<string>();

                if (user.RoleId != 0)
                {
                    var role = await UnitOfWork.Roles.GetRole(user.RoleId);
                    identity.AddClaim(new Claim(ClaimTypes.Role, role.Name));
                    roles.Add(role.Name);
                }

                var principal = new GenericPrincipal(identity, roles.ToArray());

                Thread.CurrentPrincipal = principal;

                context.Validated(identity);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", "message");
            }
        }
    }
}