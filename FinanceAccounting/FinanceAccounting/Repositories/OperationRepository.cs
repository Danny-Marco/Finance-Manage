using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Castle.Core.Internal;
using FinanceAccounting.DataBase;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Repositories
{
    public class OperationRepository : IOperationRepository
    {
        private FinanceContext _context;

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

        public void CreateOperation(Account account, Operation operation)
        {
            switch (operation.PurposeOperation)
            {
                case OperationEnum.Income:
                    account.Operations.Add(operation);
                    account.CurrentSum += operation.Sum;
                    _context.SaveChanges();
                    break;

                case OperationEnum.Expense when (account.CurrentSum - operation.Sum) > 0:
                    account.Operations.Add(operation);
                    account.CurrentSum -= operation.Sum;
                    _context.SaveChanges();
                    break;
            }
        }

        public void Delete(Operation operation)
        {
            _context.Operations.Remove(operation);
            _context.SaveChanges();
        }

        public void Update(Operation operation)
        {
            var changeOperation = Get(operation.OperationId);

            changeOperation.Account = operation.Account;
            changeOperation.AccountId = operation.AccountId;
            changeOperation.DefinitionId = operation.DefinitionId;
            changeOperation.Date = operation.Date;
            changeOperation.Description = operation.Description;
            changeOperation.Account = operation.Account;
            changeOperation.Sum = operation.Sum;
            _context.SaveChanges();
        }

        public ExpandoObject GetAccountOperations(Account account)
        {
            var income = GetOperationsByDefinitionId(account.Operations, 1);
            var expend = GetOperationsByDefinitionId(account.Operations, 1);

            return FormResponseByType(income, expend);
        }

        public ExpandoObject GetOperationsByDate(Account account, DateTime date)
        {
            var operations = account.Operations.Where(o => o.Date == date).ToList();

            if (!operations.IsNullOrEmpty())
            {
                return GetSortedOperationByType(operations);
            }

            throw new InvalidOperationException();
        }

        public ExpandoObject GetOperationsForPeriod(Account account, DateTime dateStart, DateTime dateEnd)
        {
            var operations =
                account.Operations
                    .Where(o => o.Date >= dateStart && o.Date <= dateEnd).ToList();

            if (!operations.IsNullOrEmpty())
            {
                return GetSortedOperationByType(operations);
            }

            throw new InvalidOperationException();
        }

        public ExpandoObject GetSortedOperationsByType(List<Operation> operations, int definitionId)
        {
            operations = GetOperationsByDefinitionId(operations, definitionId);
            return FormResponse(operations);
        }


        private List<Operation> GetOperationsByDefinitionId(List<Operation> operations, int definitionId)
        {
            return operations.Where(o => o.DefinitionId == definitionId).ToList();
        }

        private ExpandoObject GetSortedOperationByType(List<Operation> operations)
        {
            dynamic response = new ExpandoObject();

            var income = GetOperationsByDefinitionId(operations, 1);
            var expense = GetOperationsByDefinitionId(operations, 2);

            response.Income = FormResponse(income);
            response.Expense = FormResponse(expense);

            return response;
        }

        private ExpandoObject FormResponseByType(List<Operation> income, List<Operation> expense)
        {
            dynamic response = new ExpandoObject();

            response.Income = FormResponse(income);
            response.Expense = FormResponse(expense);

            return response;
        }

        private ExpandoObject FormResponse(List<Operation> operations)
        {
            dynamic response = new ExpandoObject();

            response.Operations = operations.Select(o
                => new
                {
                    operationId = o.OperationId,
                    definitionId = o.DefinitionId,
                    sum = o.Sum,
                    date = o.Date,
                    description = o.Description,
                    accountId = o.AccountId,
                });

            return response;
        }
    }
}