using System;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.UnitsOfWork.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IAccountRepository Accounts { get; }

        void Save();
    }
}