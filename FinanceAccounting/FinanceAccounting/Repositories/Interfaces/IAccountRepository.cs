using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.Models;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public Account GetAccount(int id);

        public List<Account> GetAllAccounts();
        
        void DeleteAccount(int accountId);
        
        public List<Operation> GetOperations(int accountId);
    }
}