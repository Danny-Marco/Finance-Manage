using System.Collections.Generic;
using FinanceAccounting.Models;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        void Create(Account account);
        
        
    }
}