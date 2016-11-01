using System;

namespace OnlineShop.Core.Data
{
    public interface IUnitOfWork : IDisposable
    {
        void Commit();
    }
}
