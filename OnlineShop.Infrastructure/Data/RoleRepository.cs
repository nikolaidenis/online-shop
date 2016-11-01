using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UnitOfWork unitOfWork;
        public RoleRepository() 
        {
            unitOfWork = new UnitOfWork();
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}
