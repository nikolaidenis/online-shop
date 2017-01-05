using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using OnlineShop.Api.Models;

namespace OnlineShop.Api.Helpers
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private static IdentityDbContext<ApplicationUser> Context { get; set; }

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var appDbContext = context.Get<IdentityDbContext<ApplicationUser>>();
            var appUserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(appDbContext))
            {
                EmailService = new EmailService()
            };
            Context = appDbContext;

            //Rest of code is removed for clarity

            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                appUserManager.UserTokenProvider =
                    new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"))
                    {
                        //Code for email confirmation and reset password life time
                        TokenLifespan = TimeSpan.FromHours(6)
                    };
            }

            return appUserManager;
        }

        public override async Task<IdentityResult> CreateAsync(ApplicationUser user, string password)
        {
            var identity = new IdentityResult();
            return identity;
        }

        public override async Task<IdentityResult> ConfirmEmailAsync(string userId, string token)
        {
            var identity = new IdentityResult();

            return identity;
        }
    }
}