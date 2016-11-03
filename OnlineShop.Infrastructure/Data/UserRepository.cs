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
            return await _unitOfWork.Users.Get(user => user.Id == id);
        }

        public async Task<User> ModifyUser(int id)
        {
            var user = await _unitOfWork.Users.Get(usr => usr.Id == id);
            await _unitOfWork.Users.Update(user);
            return user;
        }

        public async Task<bool> IsUserExist(string username)
        {
            return (await _unitOfWork.Users.Get(user => user.UserName == username)) != null;
        }

        public Task<int> CreateUser(User obj)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpdateUser(User user)
        {
            await _unitOfWork.Users.Update(user);
        }

        public async Task SaveChanges()
        {
            await _unitOfWork.CommitAsync();
        } 
    }
}
