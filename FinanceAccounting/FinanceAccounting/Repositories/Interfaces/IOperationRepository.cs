using System.Collections.Generic;
using FinanceAccounting.Models;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IOperationRepository : IRepository<Operation>
    {
        public List<Operation> GetAccountOperations(int accountId);
        
        public Account GetAccount(int id);

        public void CreateOperation(int accountId, Operation operation);
    }
}