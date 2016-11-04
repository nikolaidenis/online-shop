using System;
using System.Data.Entity;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        internal readonly DbContext Context;

        private Repository<Role> _roleRepository;
        private Repository<Product> _productRepository;
        private Repository<User> _userRepository;
        private Repository<Operation> _operationRepository; 

        public Repository<Role> Roles => _roleRepository ?? (_roleRepository = new Repository<Role>(Context));

        public Repository<Product> Products => _productRepository ?? (_productRepository = new Repository<Product>(Context));

        public Repository<User> Users => _userRepository ?? (_userRepository = new Repository<User>(Context));

        public Repository<Operation> Operations
            => _operationRepository ?? (_operationRepository = new Repository<Operation>(Context)); 

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public async Task CommitAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Commit()
        {
            Context.SaveChanges();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
