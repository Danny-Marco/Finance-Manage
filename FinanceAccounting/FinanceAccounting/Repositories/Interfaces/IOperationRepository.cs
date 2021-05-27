using System;
using System.Collections.Generic;
using System.Dynamic;
using FinanceAccounting.Models;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IOperationRepository : IRepository<Operation>
    {
        public List<Operation> GetAccountOperations(Account account);

        public Account GetAccount(int id);

        void Update(Operation foundOperation, Operation transmittedOperation);

        public void CreateOperation(Account account, Operation operation, ref bool IsAdded);

        public List<Operation> GetOperationsByDate(Account account, DateTime date, ref bool areThereOperations);
        
        public List<Operation> GetOperationsForPeriod(Account account, DateTime dateStart, DateTime dateEnd, ref bool areThereOperations);

        public List<Operation> GetSortedOperationsByType(List<Operation> operations, int definitionId);
    }
}