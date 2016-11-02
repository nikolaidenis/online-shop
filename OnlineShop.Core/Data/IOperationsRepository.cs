using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Core.Data
{
    public interface IOperationsRepository
    {
        Task MakePurchase(Operation operation); 
        Task<Operation> GetOperation(int id);
        Task<Operation> GetOperation(int id, int userId);
        Task<IEnumerable<Operation>> GetOperations(int userId);
        Task MakeSale(int purchaseId);
    }
}
