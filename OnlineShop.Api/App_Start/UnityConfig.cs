using System;
using System.Web.Http;
using Microsoft.Practices.Unity;
using OnlineShop.Core.Data;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Api.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity container.</summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // container.LoadConfiguration();
            var connectionString =
                "metadata=res://*/ShopContext.csdl|res://*/ShopContext.ssdl|res://*/ShopContext.msl;provider=System.Data.SqlClient;provider connection string='Server=WSC-028\\SQLEXPRESS;initial catalog=OnlineShop;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework'";
            // TODO: Register your types here
            container.RegisterType<IUnitOfWork>(new PerRequestLifetimeManager(), new InjectionFactory(c=>UnitOfWork.CreateContext(connectionString)));
            //container.RegisterType<IProductRepository, ProductRepository>(new PerRequestLifetimeManager(), new InjectionConstructor(shopContext));
//             container.RegisterType<IOperationsRepository, OperationsRepository>(new PerRequestLifetimeManager(), new InjectionConstructor(shopContext));
            // container.RegisterType<IRoleRepository, RoleRepository>(new PerRequestLifetimeManager(), new InjectionConstructor(shopContext));
            // container.RegisterType<IUserRepository, UserRepository>(new PerRequestLifetimeManager(), new InjectionConstructor(shopContext));

        }
    }
}
