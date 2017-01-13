using System;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
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
            var config = new HttpConfiguration();
            WebApiConfig.Register(config);

            ConfigureOAuth(app, config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app, HttpConfiguration config)
        {

            var uof = (UnitOfWork)config.DependencyResolver.GetService(typeof(IUnitOfWork));
//            var identityDbContext = new IdentityDbContext<ApplicationUser>(uof.GetContext().Database.Connection.ConnectionString);

            var options = new OAuthAuthorizationServerOptions
                        {
                            AllowInsecureHttp = true,
                            TokenEndpointPath = new PathString("/api/token"),
                            AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                            Provider = new CustomAuthorizationProvider(uof)
                        };
            
            app.UseOAuthAuthorizationServer(options);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
//            app.CreatePerOwinContext(()=>identityDbContext);
//            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
        }
    }
    
}