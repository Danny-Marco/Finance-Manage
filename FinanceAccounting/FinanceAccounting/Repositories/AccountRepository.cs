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
        private FinanceContext _context;

        public AccountRepository(FinanceContext context)
        {
            _context = context;
        }

        public void Create(Account account)
        {
            try
            {
                _context.Accounts.Add(account);
                _context.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Account Get(int id)
        {
            return _context.Accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public List<Account> GetAll()
        {
            var accounts = _context.Accounts.ToList();
            return accounts;
        }

        public void Delete(Account account)
        {
            _context.Accounts.Remove(account);
            _context.SaveChanges();
        }

        public int Count()
        {
            return _context.Accounts.Count();
        }
    }
}