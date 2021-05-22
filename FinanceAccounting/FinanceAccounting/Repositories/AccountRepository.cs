using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        #region Account

        public void Create(Account account)
        {
            _context.Accounts.Add(account);
            _context.SaveChanges();
        }

        public Account GetAccount(int id)
        {
            return _context.Accounts.Find(id);
        }

        public List<Account> GetAllAccounts()
        {
            return _context.Accounts.ToList();
        }

        public async void DeleteAccount(int accountId)
        {
            var account = GetAccount(accountId);
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        #endregion

        
        #region Operation

        public List<Operation> GetOperations(int accountId)
        {
            var account = GetAccount(accountId);
            return account.Operations;
        }

        #endregion
    }
}