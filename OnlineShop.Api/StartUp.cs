using System;
using System.Web.Http;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using OnlineShop.Api.Providers;
using OnlineShop.Core.Data;
using OnlineShop.Infrastructure.Data;
using Owin;

[assembly: OwinStartup(startupType: typeof(OnlineShop.Api.Startup))]
namespace OnlineShop.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            WebApiConfig.Register(configuration);
            ConfigureOAuth(app, configuration);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(configuration);
            
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
            //            app.CreatePerOwinContext<OwinAuthDbContext>(() => new OwinAuthDbContext());
            //            app.CreatePerOwinContext<UserManager<IdentityUser>>(CreateManager);
        }

//        private static UserManager<IdentityUser> CreateManager(IdentityFactoryOptions<UserManager<IdentityUser>> options, IOwinContext context)
//        {
//            var userStore = new UserStore<IdentityUser>(context.Get<OwinAuthDbContext>());
//            var owinManager = new UserManager<IdentityUser>(userStore);
//            return owinManager;
//        }
    }
    
}