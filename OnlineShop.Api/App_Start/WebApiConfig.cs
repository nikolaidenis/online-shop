using System.Web.Http;
using Microsoft.Practices.Unity.Configuration;
using OnlineShop.Api.App_Start;

namespace OnlineShop.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "UserIdParameterApi",
                routeTemplate: "api/{controller}/{action}/{userId}",
                defaults: new {userId = RouteParameter.Optional}
                );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            var container = UnityConfig.GetConfiguredContainer();
            config.DependencyResolver = new UnityResolver(container);
        }
    }
}
