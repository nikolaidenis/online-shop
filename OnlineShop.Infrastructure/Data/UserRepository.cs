using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class UserRepository: Repository<User>,IUserRepository
    {

        public UserRepository(DbContext context):base(context)
        {
        }
        
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await GetAll();
        }

        public async Task<User> GetUser(int id)
        {
            return await Get(user => user.Id == id);
        }

        public async Task<User> ModifyUser(int id)
        {
            var user = await Get(usr => usr.Id == id);
            await Update(user);
            return user;
        }

        public async Task<bool> IsUserExist(string username)
        {
            return (await Get(user => user.UserName == username)) != null;
        }

        public async Task<int> CreateUser(User obj)
        {
            var user = await Create(obj);
            return user.Id;
        }

        public async Task UpdateUser(User user)
        {
            await Update(user);
            await SaveChanges();
        }

        public async Task SaveChanges()
        {
            await CommitAsync();
        }

        public async Task<int> AuthenticateUser(string login, string password)
        {
            var user = await Get(usr => usr.UserName == login && usr.Password == password);
            return user?.Id ?? 0;
        }
    }
}
