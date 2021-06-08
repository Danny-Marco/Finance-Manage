using System.Collections.Generic;
using System.Linq;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Tests.TestRepositories
{
    public class TestAccountRepository : IAccountRepository
    {
        private static readonly DataTest dataTest = new DataTest();
        private readonly List<Account> Accounts = dataTest.CreateAccounts();

        public Account Get(int id)
        {
            return Accounts.FirstOrDefault(a => a.AccountId == id);
        }

        public List<Account> GetAll()
        {
            return Accounts;
        }

        public void Delete(Account account)
        {
            Accounts.Remove(account);
        }

        public void Update(Account account)
        {
            var findAccount = Get(account.AccountId);
            findAccount.CurrentSum = account.CurrentSum;
        }

        public void Add(Account account)
        {
            Accounts.Add(account);
        }

        public void Save()
        {
        }

        public int Count()
        {
            return Accounts.Count;
        }
    }
}