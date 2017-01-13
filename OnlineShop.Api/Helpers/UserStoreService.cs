using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Practices.Unity;
using OnlineShop.Api.Models;
using OnlineShop.Core.Data;

namespace OnlineShop.Api.Helpers
{
    public class UserStoreService: IUserStore<ApplicationUser>,IUserPasswordStore<ApplicationUser>
    {
        [Dependency]
        public IUnitOfWork UnitOfWork { get; }

        public UserStoreService(IUnitOfWork uof)
        {
            UnitOfWork = uof;
        }

        public void Dispose()
        {
            
        }

        public Task CreateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public Task FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            var list = await UnitOfWork.Users.GetUsers();
            var user = list.Where(p => p.UserName == userName);
            return AutoMapper.Mapper.Map<ApplicationUser>(user);
        }

        public Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            throw new NotImplementedException();
        }

        public Task GetPasswordHashAsync(ApplicationUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return Task.FromResult(user.Password);
        }

        public Task HasPasswordAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        Task<string> IUserPasswordStore<ApplicationUser, string>.GetPasswordHashAsync(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserPasswordStore<ApplicationUser, string>.HasPasswordAsync(ApplicationUser user)
        {
            return Task.FromResult(user.Password != null);
        }

        Task<ApplicationUser> IUserStore<ApplicationUser, string>.FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        Task<ApplicationUser> IUserStore<ApplicationUser, string>.FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }
    }
}