using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class OperationsRepository : Repository<Operation>, IOperationsRepository
    {

        public OperationsRepository(DbContext context):base(context)
        {
        }

        public async Task MakePurchase(Operation operation)
        {
            await Create(operation);
            await CommitAsync();
        }

        public async Task<Operation> GetOperation(int id)
        {
            return await Get(op => op.Id == id);
        }

        public Task<Operation> GetOperation(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Operation>> GetOperations(int userId)
        {
            var list = GetAllAsQueryable();
            return await list.Where(p => p.UserID == userId).ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Operation>> GetOperations(int userId, int page, int rowsCount)
        {
            var list = GetAllAsQueryable();
            return await list.Where(p => p.UserID == userId).OrderBy(p=>p.Id)
                .Skip(rowsCount*page).Take(rowsCount).ToListAsync().ConfigureAwait(false);
        }

        public async Task MakeSale(int purchaseId)
        {
            (await GetOperation(purchaseId)).IsSelled = true;
            Commit();
        }
    }
}
