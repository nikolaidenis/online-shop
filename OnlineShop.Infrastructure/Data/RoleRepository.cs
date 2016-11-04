using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class RoleRepository : IRoleRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public RoleRepository(DbContext context) 
        {
            _unitOfWork = new UnitOfWork(context);
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}
