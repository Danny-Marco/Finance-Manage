using System;
using FinanceAccounting.DataBase;
using FinanceAccounting.Repositories;
using FinanceAccounting.Repositories.Interfaces;
using FinanceAccounting.UnitsOfWork.Interfaces;

namespace FinanceAccounting.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private IFinanceContext _db;
        private bool disposed;

        public IAccountRepository Accounts { get; }
        
        public IOperationRepository Operations { get; }
        
        public UnitOfWork(IFinanceContext context)
        {
            _db = context;
            Accounts = new AccountRepository(_db);
            Operations = new OperationRepository(_db);
        }
        
        public void Save()
        {
            _db.Save();
        }

        #region Disposing

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}