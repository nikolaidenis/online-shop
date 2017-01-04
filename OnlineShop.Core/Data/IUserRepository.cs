using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUser(int id);
        Task<User> ModifyUser(int id);
        Task<bool> IsUserExist(string username);
        Task<int> CreateUser(User obj);
        Task UpdateUser(User user);
        Task SaveChanges();
        Task<int> AuthenticateUser(string login, string password);
    }
}
