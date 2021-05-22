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

        public ExpandoObject GetAccountOperations(Account account)
        {
            var income = GetOperationsByType(account, OperationEnum.Income);
            var expend = GetOperationsByType(account, OperationEnum.Expense);
            
            return GetSortedOperationByType(income, expend);
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
        
        public void CreateOperation(int accountId, Operation operation)
        {
            var account = GetAccount(accountId);

            // TODO реализовать обработку операций через Observer
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
            _context.SaveChanges();
        }


        private ExpandoObject GetSortedOperationByType(IReadOnlyCollection<Operation> operations)
        {
            dynamic response = new ExpandoObject();

            var income = 
                operations.Where(o => o.PurposeOperation == OperationEnum.Income).ToList();
            
            var expends = 
                operations.Where(o => o.PurposeOperation == OperationEnum.Expense).ToList();

            response.Income = income.Select(o
                => new
                {
                    operationId = o.OperationId,
                    definitionId = o.DefinitionId,
                    accountId = o.AccountId,
                    sum = o.Sum,
                    description = o.Description,
                    date = o.Date
                });

            response.Expend = expends.Select(o
                => new
                {
                    operationId = o.OperationId,
                    definitionId = o.DefinitionId,
                    accountId = o.AccountId,
                    sum = o.Sum,
                    description = o.Description,
                    date = o.Date
                });

            return response;
        }
        
        private ExpandoObject GetSortedOperationByType(List<Operation> income, List<Operation> extends)
        {
            dynamic response = new ExpandoObject();

            response.Income = income.Select(o
                => new
                {
                    operationId = o.OperationId,
                    definitionId = o.DefinitionId,
                    accountId = o.AccountId,
                    sum = o.Sum,
                    description = o.Description,
                    date = o.Date
                });

            response.Expend = extends.Select(o
                => new
                {
                    operationId = o.OperationId,
                    definitionId = o.DefinitionId,
                    accountId = o.AccountId,
                    sum = o.Sum,
                    description = o.Description,
                    date = o.Date
                });

            return response;
        }

        private List<Operation> GetOperationsByType(Account account, OperationEnum operationType)
        {
            return account.Operations.Where(o => o.PurposeOperation == operationType).ToList();
        }
    }
}