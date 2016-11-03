using System;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
        Task CommitAsync();
    }
}
