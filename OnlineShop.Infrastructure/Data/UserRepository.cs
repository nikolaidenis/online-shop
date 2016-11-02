using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class UserRepository: Repository<User>,IUserRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public UserRepository()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _unitOfWork.Users.GetAll();
        }

        public async Task<User> GetUser(int id)
        {
            return await _unitOfWork.Users.Get(id);
        }

        public async Task<bool> IsUserExist(string username)
        {
            return (await _unitOfWork.Users.Find(user => user.UserName == username)) != null;
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
