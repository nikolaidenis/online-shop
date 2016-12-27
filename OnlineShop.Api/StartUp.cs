using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
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
            ConfigureOAuth(app);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
                        var options = new OAuthAuthorizationServerOptions
                        {
                            AllowInsecureHttp = true,
                            TokenEndpointPath = new PathString("/token"),
                            AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                            Provider = new CustomAuthorizationProvider()
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