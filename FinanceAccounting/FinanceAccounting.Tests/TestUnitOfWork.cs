using FinanceAccounting.Repositories.Interfaces;
using FinanceAccounting.Tests.TestRepositories;
using FinanceAccounting.UnitsOfWork.Interfaces;

namespace FinanceAccounting.Tests
{
    public class TestUnitOfWork : IUnitOfWork
    {
        private TestAccountRepository _accountRepository;

        private TestOperationRepository _operationRepository;

        public IAccountRepository Accounts
        {
            get
            {
                if (_accountRepository != null) return _accountRepository;
                _accountRepository = new TestAccountRepository();
                return _accountRepository;
            }
        }

        public IOperationRepository Operations
        {
            get
            {
                if (_operationRepository != null) return _operationRepository;
                _operationRepository = new TestOperationRepository();
                return _operationRepository;
            }
        }

        public void Save()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}