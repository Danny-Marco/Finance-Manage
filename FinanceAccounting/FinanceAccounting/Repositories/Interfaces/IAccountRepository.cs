using System.Collections.Generic;
using System.Threading.Tasks;
using FinanceAccounting.Models;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        void Create(Account account);

        public int Count();
    }
}