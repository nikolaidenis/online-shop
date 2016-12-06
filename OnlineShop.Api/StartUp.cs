using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using OnlineShop.Api.Providers;
using OnlineShop.Infrastructure.Data;
using Owin;

[assembly: OwinStartup(startupType: typeof(OnlineShop.Api.StartUp))]
namespace OnlineShop.Api
{
    public class StartUp
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            app.UseWebApi(configuration);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            var options = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new CustomAuthorizationProvider(new UnitOfWork());
            };
        }
    }
}