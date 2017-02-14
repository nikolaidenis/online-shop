using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Threading;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using OnlineShop.Api.Handlers;
using OnlineShop.Api.Models;
using OnlineShop.Api.Providers;
using OnlineShop.Core.Data;
using OnlineShop.Infrastructure.Data;
using Owin;

[assembly: OwinStartup(typeof(OnlineShop.Api.Startup))]
namespace OnlineShop.Api
{
    public class Startup
    {
        
        public void Configuration(IAppBuilder app)
        {
            
            app.MapSignalR();
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);
            ConfigureOAuth(app, config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(config);
            ConfigureDependencyHandler(config);
            OnShutdown(app, config);
        }

        public void ConfigureOAuth(IAppBuilder app, HttpConfiguration config)
        {
            var uof = (UnitOfWork)config.DependencyResolver.GetService(typeof(IUnitOfWork));

            var options = new OAuthAuthorizationServerOptions
                        {
                            AllowInsecureHttp = true,
                            TokenEndpointPath = new PathString("/api/token"),
                            AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                            Provider = new CustomAuthorizationProvider(uof)
                        };
            
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        private void ConfigureDependencyHandler(HttpConfiguration config)
        {
            var uof = (UnitOfWork)config.DependencyResolver.GetService(typeof(IUnitOfWork));
            SqlDependencyHandler.Register(uof.GetConnectionString());
        }

        public void OnShutdown(IAppBuilder app, HttpConfiguration config)
        {
            var context = new OwinContext(app.Properties);
            var token = context.Get<CancellationToken>("host.OnAppDisposing");
            if (token != CancellationToken.None)
            {
                var uof = (UnitOfWork)config.DependencyResolver.GetService(typeof(IUnitOfWork));
                token.Register(() =>
                {
                    SqlDependency.Stop(uof.GetConnectionString());
                });
            }
        }
    }
    
}