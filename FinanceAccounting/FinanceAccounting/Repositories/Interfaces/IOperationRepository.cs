using System;
using System.Collections.Generic;
using FinanceAccounting.Models;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IOperationRepository : IRepository<Operation>
    {
        public List<Operation> GetOperationsByDate(Account account, DateTime date);

        public List<Operation> GetOperationsForPeriod(Account account, DateTime dateStart, DateTime dateEnd);

        public List<Operation> GetOperationsByType(List<Operation> operations, int definitionId);
    }
}