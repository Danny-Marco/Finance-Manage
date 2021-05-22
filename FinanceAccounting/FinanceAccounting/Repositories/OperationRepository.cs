using System.Collections.Generic;
using System.Linq;
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

        public Operation Get(int id)
        {
            return _context.Operations.FirstOrDefault(operation => operation.OperationId == id);
        }

        public List<Operation> GetAll()
        {
            return _context.Operations.ToList();
        }

        public List<Operation> GetAccountOperations(int accountId)
        {
            return GetAccount(accountId).Operations;
        }

        public Account GetAccount(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountId == id);
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

        public void Delete(int operationId)
        {
            var operation = Get(operationId);
            _context.Operations.Remove(operation);
            _context.SaveChanges();
        }

        public void Update(Operation operation)
        {
            throw new System.NotImplementedException();
        }
    }
}