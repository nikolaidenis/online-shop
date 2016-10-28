using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class UnitOfWorkRepository : IUnitOfWork
    {
        private ITransaction transaction;

        private IRoleRepository roleRepository;
        private IProductRepository productRepository;
        private IUserRepository userRepository;
        public ISession Session { get; private set; }

        public UnitOfWorkRepository()
        {
                
        }

        public IRoleRepository Roles
        {
            get
            {
                if (roleRepository == null)
                {
                    roleRepository = new RoleRepository();
                }
                return roleRepository;
            }
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}
