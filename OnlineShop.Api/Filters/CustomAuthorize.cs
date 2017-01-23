using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace OnlineShop.Api.Filters
{
    public class CustomAuthorizeAttribute: ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var authorization = actionContext.Request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer")
            {
                return Task.FromResult(0);
            }

            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }
    }
}