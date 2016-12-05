using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(DbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Role>> GetRoles()
        {
            throw new NotImplementedException();
        }
    }
}
