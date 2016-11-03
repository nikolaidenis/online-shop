using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using OnlineShop.Core;
using OnlineShop.Core.Data;

namespace OnlineShop.Infrastructure.Data
{
    public class OperationsRepository : IOperationsRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public OperationsRepository()
        {
            _unitOfWork = new UnitOfWork();
        }

        public async Task MakePurchase(Operation operation)
        {
            await _unitOfWork.Operations.Create(operation);
            await _unitOfWork.CommitAsync();
        }

        public async Task<Operation> GetOperation(int id)
        {
            return await _unitOfWork.Operations.Get(op => op.Id == id);
        }

        public Task<Operation> GetOperation(int id, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Operation>> GetOperations(int userId)
        {
            var list = _unitOfWork.Operations.GetAllAsQueryable();
            return await list.Where(p => p.UserID == userId).ToListAsync();
        }

        public async Task MakeSale(int purchaseId)
        {
            (await GetOperation(purchaseId)).IsSelled = true;
            _unitOfWork.Commit();
        }
    }
}
