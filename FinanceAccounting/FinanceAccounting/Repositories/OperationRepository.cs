using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using FinanceAccounting.DataBase;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Repositories
{
    public class OperationRepository : IOperationRepository
    {
        private readonly FinanceContext _context;

        public OperationRepository(FinanceContext context)
        {
            _context = context;
        }

        public Account GetAccount(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public Operation Get(int id)
        {
            return _context.Operations.FirstOrDefault(operation => operation.OperationId == id);
        }

        public List<Operation> GetAll()
        {
            return _context.Operations.ToList();
        }

        public void CreateOperation(Account account, Operation operation, ref bool isAdded)
        {
            switch (operation.PurposeOperation)
            {
                case OperationEnum.Income:
                    AddToAccountOperations(account, operation, out isAdded);
                    account.CurrentSum += operation.Sum;
                    break;

                case OperationEnum.Expense when (account.CurrentSum - operation.Sum) > 0:
                    AddToAccountOperations(account, operation, out isAdded);
                    account.CurrentSum -= operation.Sum;
                    break;
            }
        }

        private void AddToAccountOperations(Account account, Operation operation, out bool isAdded)
        {
            account.Operations.Add(operation);
            
            _context.SaveChanges();
            isAdded = true;
        }

        public void Delete(Operation operation)
        {
            _context.Operations.Remove(operation);
            _context.SaveChanges();
        }

        public void Update(Operation foundOperation, Operation transmittedOperation)
        {
            foundOperation.DefinitionId = transmittedOperation.DefinitionId;
            foundOperation.Sum = transmittedOperation.Sum;
            foundOperation.Date = transmittedOperation.Date;
            foundOperation.Description = transmittedOperation.Description;
            _context.SaveChanges();
        }

        public List<Operation> GetAccountOperations(Account account)
        {
            return account.Operations.ToList();
        }

        public List<Operation> GetOperationsByDate(Account account, DateTime date, ref bool areThereOperations)
        {
            var operations =
                account.Operations.Where(o => o.Date.Day == date.Date.Day).ToList();

            if (!operations.IsNullOrEmpty())
            {
                areThereOperations = true;
            }

            return operations;
        }

        public List<Operation> GetOperationsForPeriod(Account account, DateTime dateStart, DateTime dateEnd,
            ref bool areThereOperations)
        {
            var operations =
                account.Operations
                    .Where(o => o.Date.Day >= dateStart.Date.Day && o.Date.Day <= dateEnd.Date.Day).ToList();

            if (!operations.IsNullOrEmpty())
            {
                areThereOperations = true;
            }

            return operations;
        }

        public List<Operation> GetSortedOperationsByType(List<Operation> operations, int definitionId)
        {
            operations = operations.Where(o => o.DefinitionId == definitionId).ToList();
            return operations;
        }
    }
}