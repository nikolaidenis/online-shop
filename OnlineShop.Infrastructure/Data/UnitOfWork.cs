using System;
using System.Data.Entity;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class UnitOfWork : DbContext, IUnitOfWork
    {
        internal readonly DbContext Context;

        private IRoleRepository _roleRepository;
        private IProductRepository _productRepository;
        private IUserRepository _userRepository;
        private IOperationsRepository _operationRepository;
        private IUserSessionsRepository _userSessionsRepository;

        public IRoleRepository Roles => _roleRepository ?? (_roleRepository = new RoleRepository(Context));

        public IProductRepository Products => _productRepository ?? (_productRepository = new ProductRepository(Context));

        public IUserRepository Users => _userRepository ?? (_userRepository = new UserRepository(Context));

        public IOperationsRepository Operations 
            => _operationRepository ?? (_operationRepository = new OperationsRepository(Context));

        public IUserSessionsRepository UserSessions 
            => _userSessionsRepository ?? (_userSessionsRepository = new UserSessionsRepository(Context));

        public static IUnitOfWork CreateContext(string connectionString)
        {
            return new UnitOfWork(connectionString);
        }

        public DbContext GetContext()
        {
            return Context;
        }

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }
        public UnitOfWork(string connectionString)
        {
            Context = new DbContext(connectionString);
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

        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
