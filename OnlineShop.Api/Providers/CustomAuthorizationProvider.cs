using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
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
            context.OwinContext.Response.Headers.Add("Access-Control-Allow-Origin", new[] { "*" });

            var userId = await UnitOfWork.Users.AuthenticateUser(context.UserName, context.Password);
            if (userId == 0)
            {
                context.SetError("invalid_grant", "Invalid User Id or password'");
                context.Rejected();
                return;
            }

            var message = new IdentityMessage();
            

            var identity = new ClaimsIdentity(context.Options.AuthenticationType);
            identity.AddClaim(new Claim("sub", context.UserName));
            identity.AddClaim(new Claim("role", "user"));

            context.Validated(identity);
        }
    }
}