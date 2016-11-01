using System;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ShopContext context;

        private Repository<Role> roleRepository;
        private Repository<Product> productRepository;
        private Repository<User> userRepository;

        public Repository<Role> Roles => roleRepository ?? (roleRepository = new Repository<Role>(context));

        public Repository<Product> Products => productRepository ?? (productRepository = new Repository<Product>(context));

        public Repository<User> Users => userRepository ?? (userRepository = new Repository<User>(context));

        public UnitOfWork()
        {
            context = new ShopContext();
        }

        public void Commit()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
