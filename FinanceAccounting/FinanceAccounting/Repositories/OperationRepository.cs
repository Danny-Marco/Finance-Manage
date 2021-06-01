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
        private readonly IFinanceContext _context;

        public OperationRepository(IFinanceContext context)
        {
            _context = context;
        }

        public Operation Get(int id)
        {
            return _context.Operations.Find(id);
        }

        public List<Operation> GetAll()
        {
            return _context.Operations.ToList();
        }

        public void Delete(Operation operation)
        {
            _context.Operations.Remove(operation);
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

        public void Add(Operation operation)
        {
            _context.Operations.Add(operation);
        }

        public void Save()
        {
            _context.Save();
        }

        public List<Operation> GetOperationsByDate(Account account, DateTime date)
        {
            return account.Operations.Where(o => o.Date.Day == date.Date.Day).ToList();;
        }

        public List<Operation> GetOperationsForPeriod(Account account, DateTime dateStart, DateTime dateEnd)
        {
            return account.Operations
                .Where(o => o.Date.Day >= dateStart.Date.Day && o.Date.Day <= dateEnd.Date.Day).ToList();;
        }

        public List<Operation> GetOperationsByType(List<Operation> operations, int definitionId)
        {
            operations = operations.Where(o => o.DefinitionId == definitionId).ToList();
            return operations;
        }
    }
}