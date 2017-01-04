using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;

namespace OnlineShop.Api.Models
{
    public class IdentityModels: UserManager<ApplicationUser>
    {
        public IdentityModels(IUserStore<ApplicationUser> store) : base(store)
        {
        }
    }
}