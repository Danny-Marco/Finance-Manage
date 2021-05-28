using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Castle.Core.Internal;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Tests.TestRepositories
{
    public class TestOperationRepository : IOperationRepository
    {
        private static DataTest dataTest = new DataTest();
  
        public readonly List<Account> Accounts = dataTest.CreateAccounts();
        
        public readonly List<Operation> Operations = dataTest.CreateOperations();
        
        
        public Operation Get(int id)
        {
            return Operations.FirstOrDefault(o => o.OperationId == id);
        }

        public List<Operation> GetAll()
        {
            return Operations;
        }

        public void Delete(Operation operation)
        {
            Operations.Remove(operation);
        }

        public List<Operation> GetAccountOperations(Account account)
        {
            return account.Operations;
        }

        public Account GetAccount(int id)
        {
            return Accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public void Update(Operation foundOperation, Operation transmittedOperation)
        {
            foundOperation.DefinitionId = transmittedOperation.DefinitionId;
            foundOperation.Sum = transmittedOperation.Sum;
            foundOperation.Date = transmittedOperation.Date;
            foundOperation.Description = transmittedOperation.Description;
        }

        public void CreateOperation(Account account, Operation operation, ref bool IsAdded)
        {
            switch (operation.PurposeOperation)
            {
                case OperationEnum.Income:
                    AddToAccountOperations(account, operation, out IsAdded);
                    account.CurrentSum += operation.Sum;
                    break;

                case OperationEnum.Expense when (account.CurrentSum - operation.Sum) > 0:
                    AddToAccountOperations(account, operation, out IsAdded);
                    account.CurrentSum -= operation.Sum;
                    break;
            }
        }
        
        private void AddToAccountOperations(Account account, Operation operation, out bool isAdded)
        {
            account.Operations.Add(operation);
            isAdded = true;
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

        public List<Operation> GetOperationsForPeriod(Account account, DateTime dateStart, DateTime dateEnd, ref bool areThereOperations)
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