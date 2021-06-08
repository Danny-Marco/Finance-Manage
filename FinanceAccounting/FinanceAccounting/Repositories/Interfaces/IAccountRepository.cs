using FinanceAccounting.Models;

namespace FinanceAccounting.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        public int Count();
    }
}