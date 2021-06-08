using System;
using System.Collections.Generic;
using FinanceAccounting.Models;
using FinanceAccounting.Models.Interfaces;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.UnitsOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }

        IOperationRepository Operations { get; }

        Account GetAccountById(int id);

        List<Account> GetAllAccounts();

        Operation GetOperationByID(int id);

        void RegisterDirty(IEntity entity);

        void RegisterDelete(IEntity entity);

        void RegisterNew(IEntity entity, ref bool isAdded);

        List<Operation> GetOperationsByDate(Account account, DateTime date, ref bool areThereOperations);

        List<Operation> GetOperationsByType(List<Operation> operations, int definitionId);

        List<Operation> GetOperationsForPeriod(Account account, DateTime startDate, DateTime endDate,
            ref bool areThereOperations);

        void Commit();

        void Disposing(IEntity entity);
    }
}