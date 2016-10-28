using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public Task<IEnumerable<Role>> GetRoles()
        {
            return Context
        }
    }
}
