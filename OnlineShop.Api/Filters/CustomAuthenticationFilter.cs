using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using OnlineShop.Core.Data;

namespace OnlineShop.Api.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CustomAuthenticationFilter : Attribute, IAuthenticationFilter
    {
        public bool AllowMultiple => false;


        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            var request = context.Request;
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Custom")
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationErrorResult("Missing credentials", request);
                return;
            }

            //            Tuple<string, string> loginAndPassword = ExtractUserNameAndPassword(authorization.Parameter);
            Tuple<string, string> loginAndPassword = new Tuple<string, string>("", "");
            if (loginAndPassword == null)
            {
                context.ErrorResult = new AuthenticationErrorResult("Invalid credentials", request);
                return;
            }

            var login = loginAndPassword.Item1;
            var password = loginAndPassword.Item2;

            var principal = await AuthenticateAsync(login, password, cancellationToken);
            if (principal == null)
            {
                context.ErrorResult = new AuthenticationErrorResult("Invalid username or password", request);
            }
            else
            {
                context.Principal = principal;
            }
        }

        private async Task<IPrincipal> AuthenticateAsync(string login, string password, CancellationToken token)
        {
            var principals = new CustomPrincipal();

            return principals;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            //            throw new NotImplementedException();
            return null;

        }
    }
}