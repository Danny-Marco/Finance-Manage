using System;
using FinanceAccounting.DataBase;
using FinanceAccounting.Repositories;
using FinanceAccounting.Repositories.Interfaces;
using FinanceAccounting.UnitsOfWork.Interfaces;

namespace FinanceAccounting.UnitsOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private FinanceContext _db = new FinanceContext();
        private IAccountRepository _accountRepository;
        private bool disposed;

        public IAccountRepository Accounts
        {
            get
            {
                if (_accountRepository != null) return _accountRepository;
                _accountRepository = new AccountRepository(_db);
                return (AccountRepository) _accountRepository;
            }
        }
        
        public void Save()
        {
            _db.SaveChanges();
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