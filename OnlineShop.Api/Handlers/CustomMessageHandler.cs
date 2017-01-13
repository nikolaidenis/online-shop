using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using OnlineShop.Core.Data;

namespace OnlineShop.Api.Handlers
{
    public class CustomMessageHandler : DelegatingHandler
    {
        [Dependency]
        public IUnitOfWork UnitOfWork { get; }

        public CustomMessageHandler(IUnitOfWork uof)
        {
            UnitOfWork = uof;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var authorization = request.Headers.Authorization;

            if (authorization == null || authorization.Scheme != "Bearer" || string.IsNullOrEmpty(authorization.Parameter))
            {
                return SendUnauthorizedResponse();
            }

            return base.SendAsync(request, cancellationToken);
        }

        private Task<HttpResponseMessage> SendUnauthorizedResponse()
        {
            var tsc = new TaskCompletionSource<HttpResponseMessage>();
            tsc.SetResult(new HttpResponseMessage(HttpStatusCode.Unauthorized));
            return tsc.Task;
        } 
    }
}