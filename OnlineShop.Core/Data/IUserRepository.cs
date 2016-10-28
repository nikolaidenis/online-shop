using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<int> CreateUser(User obj);
        Task UpdateUser(User user);
    }
}
