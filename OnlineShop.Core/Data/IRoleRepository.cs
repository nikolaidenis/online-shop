using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> GetRoles();
    }
}
