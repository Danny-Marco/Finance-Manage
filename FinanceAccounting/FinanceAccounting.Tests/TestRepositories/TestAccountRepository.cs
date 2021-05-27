using System.Collections.Generic;
using System.Linq;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Tests.TestRepositories
{
    public class TestAccountRepository : IAccountRepository
    {
        private static DataTest dataTest = new DataTest();
        public List<Account> Accounts = dataTest.CreateAccounts();
        
        public Account Get(int id)
        {
            return Accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public List<Account> GetAll()
        {
            var accounts = Accounts;
            return accounts;
        }

        public void Delete(Account account)
        {
            Accounts.Remove(account);
        }

        public void Create(Account account)
        {
            Accounts.Add(account);
        }

        public int Count()
        {
            return Accounts.Count;
        }
    }
}