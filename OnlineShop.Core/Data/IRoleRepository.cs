using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRoles();
        Task<Role> GetRole(int id);
    }
}
