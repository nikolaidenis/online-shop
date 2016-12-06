using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                return;
            }

            if (string.IsNullOrEmpty(authorization.Parameter))
            {
                context.ErrorResult = new AuthenticationErrorResult("Missing credentials", request);
                return;
            }

            var loginAndPassword = ExtractUsernameAndPassword(authorization.Parameter);
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

        private Tuple<string, string> ExtractUsernameAndPassword(string parameter)
        {
            return null;
        }

        private async Task<IPrincipal> AuthenticateAsync(string login, string password, CancellationToken token)
        {
            var principals = new CustomPrincipal();

            return principals;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            var challenge = new AuthenticationHeaderValue("Bearer");
            context.Result = new AddChallengeToUnauthorizedUser(challenge, context.Result);
            return Task.FromResult(0);
        }
    }
}