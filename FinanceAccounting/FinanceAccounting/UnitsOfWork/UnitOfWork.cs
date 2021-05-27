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
        private IOperationRepository _operationRepository;
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

        public IOperationRepository Operations
        {
            get
            {
                if (_operationRepository != null) return _operationRepository;
                _operationRepository = new OperationRepository(_db);
                return _operationRepository;
            }
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