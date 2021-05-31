using System;
using Microsoft.EntityFrameworkCore;

namespace FinanceAccounting.DataBase
{
    public interface IDbContext : IDisposable
    {
        public DbContext Instance { get; }
    }
}