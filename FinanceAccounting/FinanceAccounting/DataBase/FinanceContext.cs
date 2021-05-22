using FinanceAccounting.Models;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.DataBase
{
    public class FinanceContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Operation> Operations { get; set; }


        public FinanceContext(DbContextOptions<FinanceContext> options)
            : base(options)
        {
        }
    }
}