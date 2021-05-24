using System;
using System.Collections.Generic;
using System.Dynamic;
using FinanceAccounting.Models;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IOperationRepository : IRepository<Operation>
    {
        public ExpandoObject GetAccountOperations(Account account);
        
        public Account GetAccount(int id);
        
        void Update(Operation operation);

        public void CreateOperation(Account account, Operation operation);

        public ExpandoObject GetOperationsByDate(Account account, DateTime date);
        
        public ExpandoObject GetOperationsByPeriod(Account account, DateTime dateStart, DateTime dateEnd);
    }
}