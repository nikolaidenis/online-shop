using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace OnlineShop.Api.Filters
{
    public class AuthenticationErrorResult : IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _message;

        public AuthenticationErrorResult(string message, HttpRequestMessage request)
        {
            _message = message;
            _request = request;
        }

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(Execute());
        }
        public HttpResponseMessage Execute()
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = _request,
                ReasonPhrase = _message
            };
            return response;
        }
    }
}