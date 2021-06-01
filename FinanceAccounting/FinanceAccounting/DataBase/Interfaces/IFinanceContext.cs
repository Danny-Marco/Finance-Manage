using FinanceAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.DataBase
{
    public interface IFinanceContext : IDbContext
    {
        DbSet<Account> Accounts { get; }

        DbSet<Operation> Operations { get; }

        void Save();
    }
}