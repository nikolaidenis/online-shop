using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class UserRepository: Repository<User>,IUserRepository 
    {
        public UserRepository(DbContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await GetAll();
        }

        public async Task<User> GetUser(int id)
        {
            return await Get(id);
        }

        public Task<int> CreateUser(User obj)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateUser(User user)
        {
            throw new System.NotImplementedException();
        }
    }
}
