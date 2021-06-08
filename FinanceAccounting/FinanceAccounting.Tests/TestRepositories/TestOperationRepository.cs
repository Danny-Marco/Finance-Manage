using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Tests.TestRepositories
{
    public class TestOperationRepository : IOperationRepository
    {
        private static readonly DataTest dataTest = new DataTest();
        
        private readonly List<Operation> Operations = dataTest.CreateOperations();


        #region Get

        public Operation Get(int id)
        {
            return Operations.FirstOrDefault(o => o.OperationId == id);
        }

        public List<Operation> GetAll()
        {
            return Operations;
        }

        public List<Operation> GetOperationsByDate(Account account, DateTime date)
        {
            return account.Operations.Where(o => o.Date.Day == date.Date.Day).ToList();;
        }

        public List<Operation> GetOperationsForPeriod(Account account, DateTime dateStart, DateTime dateEnd)
        {
            return account.Operations
                .Where(o => o.Date.Day >= dateStart.Date.Day && o.Date.Day <= dateEnd.Date.Day).ToList();
        }

        public List<Operation> GetOperationsByType(List<Operation> operations, int definitionId)
        {
            operations = operations.Where(o => o.DefinitionId == definitionId).ToList();
            return operations;
        }

        #endregion

        public void Add(Operation operation)
        {
            Operations.Add(operation);
        }

        public void Save()
        {
        }

        public void Update(Operation operation)
        {
            var findOperation = Get(operation.OperationId);
            if (operation.Sum > 0)
            {
                findOperation.Sum = operation.Sum;
            }

            if (operation.DefinitionId == (int) OperationEnum.Income ||
                operation.DefinitionId == (int) OperationEnum.Expense)
            {
                findOperation.DefinitionId = operation.DefinitionId;
            }

            if (operation.Date != null && operation.Date != DateTime.MinValue)
            {
                findOperation.Date = operation.Date;
            }

            if (operation.Description != null)
            {
                findOperation.Description = operation.Description;
            }
        }

        public void Delete(Operation operation)
        {
            Operations.Remove(operation);
        }
    }
}