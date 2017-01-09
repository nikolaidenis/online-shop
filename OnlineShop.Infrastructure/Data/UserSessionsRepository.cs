using OnlineShop.Core;
using OnlineShop.Core.Data;
using System;
using System.Data.Entity;
using System.Threading.Tasks;

namespace OnlineShop.Infrastructure.Data
{
    public class UserSessionsRepository : Repository<UserSession>, IUserSessionsRepository
    {
        public UserSessionsRepository(DbContext context) : base(context)
        {
        }

        public async Task CreateNewSession(int userId, string session)
        {
            await Create(new UserSession { UserId = userId, Token = session, IsExpired = false });
        }

        public async Task<int> GetActiveUserBySession(string session)
        {
            var sessionObj = await Get(p => p.Token == session && !p.IsExpired);
            return sessionObj == null ? 0 : sessionObj.UserId;
        }

        public async Task SetSessionExpired(string session)
        {
            var sessionObj = await Get(p => p.Token == session);
            if (sessionObj == null) return;
            sessionObj.IsExpired = true;
            await Update(sessionObj);
            await CommitAsync();
        }
    }
}
