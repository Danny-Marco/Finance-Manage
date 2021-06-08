using System;
using System.Collections.Generic;
using System.Linq;
using FinanceAccounting.DataBase;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly FinanceContext _context;

        public AccountRepository(FinanceContext context)
        {
            _context = context;
        }

        public void Update(Account account)
        {
            var foundAccount = Get(account.AccountId);
            if (account.CurrentSum >= 0)
            {
                foundAccount.CurrentSum = account.CurrentSum;
            }
        }

        public void Add(Account account)
        {
            try
            {
                _context.Accounts.Add(account);
                _context.Save();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Account Get(int id)
        {
            return _context.Accounts.Find(id);
        }

        public List<Account> GetAll()
        {
            var accounts = _context.Accounts.ToList();
            return accounts;
        }

        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
            _context.Save();
        }

        public void Save()
        {
            _context.Save();
        }

        public int Count()
        {
            return _context.Accounts.Count();
        }
    }
}