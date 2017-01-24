using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
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

                var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email)
                };

                var identity = new ClaimsIdentity(claims, context.Options.AuthenticationType);
                var roles = new List<string>();

                if (user.RoleId != 0)
                {
                    var role = await UnitOfWork.Roles.GetRole(user.RoleId);
                    var trimmed = role.Name.Trim();
                    identity.AddClaim(new Claim(ClaimTypes.Role, trimmed));
                    roles.Add(trimmed);
                }

                var principal = new GenericPrincipal(identity, roles.ToArray());
                Thread.CurrentPrincipal = principal;
                
                var ticket = new AuthenticationTicket(identity,CreateProperties(context.UserName,identity, user.IsBlocked));

                context.Validated(ticket);
            }
            catch (Exception ex)
            {
                context.SetError("invalid_grant", "message");
            }
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }

        public static AuthenticationProperties CreateProperties(string userName, ClaimsIdentity oAuthIdentity, bool isBlocked)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                {"username", userName},
                {"isBlocked", isBlocked.ToString()},
                {
                    "role",
                    string.Join(",",
                        oAuthIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray())
                }

            };
            return new AuthenticationProperties(data);
        }
    }
}