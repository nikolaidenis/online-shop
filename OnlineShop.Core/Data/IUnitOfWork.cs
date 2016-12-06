using System;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        IRoleRepository Roles { get;  }
        IProductRepository Products { get;  }
        IUserRepository Users { get; }
        IOperationsRepository Operations { get;  }
//        IUnitOfWork CreateContext(string connectionString);
    }
}
