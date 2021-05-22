using System;
using System.IO;
using FinanceAccounting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinanceAccounting.DataBase
{
    public class FinanceContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public FinanceContext()
        {
            Console.WriteLine();
            Console.WriteLine("=====================");
            Console.WriteLine("Пустой конструктор!!!");
            Console.WriteLine("=====================");
            Console.WriteLine();
        }

        public FinanceContext(DbContextOptions<FinanceContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseLazyLoadingProxies();

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();
                var connectionString = configuration.GetConnectionString("sqlConnection");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }
    }
}