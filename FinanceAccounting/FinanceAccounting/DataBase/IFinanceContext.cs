using FinanceAccounting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FinanceAccounting.DataBase
{
    public interface IFinanceContext : IDbContext
    {
        DbSet<Account> Accounts { get; set; }
        
        DbSet<Operation> Operations { get; set; }

        void Save();
    }
}