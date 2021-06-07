using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using FinanceAccounting.Models;
using FinanceAccounting.Models.Interfaces;
using FinanceAccounting.Observer;
using FinanceAccounting.Observer.Interfaces;
using FinanceAccounting.Repositories.Interfaces;
using FinanceAccounting.UnitsOfWork.Interfaces;

namespace FinanceAccounting.Tests
{
    public class TestUnitOfWork : IUnitOfWork
    {
        private readonly AccountHandle _accountHandler;
        private readonly OperationHandle _operationHandler;

        public IAccountRepository Accounts { get; }

        public IOperationRepository Operations { get; }

        public TestUnitOfWork(IAccountRepository accounts, IOperationRepository operations)
        {
            Accounts = accounts;
            Operations = operations;

            _accountHandler = new AccountHandle(Accounts);
            _operationHandler = new OperationHandle(Operations, Accounts);
        }


        #region Get

        public Account GetAccountById(int id)
        {
            var account = _accountHandler.GetById(id);
            if (account == null)
            {
                account = Accounts.Get(id);
                _accountHandler.AddToStored(account);
            }

            return account;
        }

        public List<Account> GetAllAccounts()
        {
            var accounts = Accounts.GetAll();
            if (!accounts.IsNullOrEmpty())
            {
                _accountHandler.AddToStored(accounts);
            }

            return accounts;
        }

        public Operation GetOperationByID(int id)
        {
            var operation = _operationHandler.GetById(id);
            if (operation == null)
            {
                operation = Operations.Get(id);
                _operationHandler.AddToStored(operation);
            }

            return operation;
        }

        public List<Operation> GetOperationsByDate(Account account, DateTime date, ref bool areThereOperations)
        {
            var operations = Operations.GetOperationsByDate(account, date);
            if (!operations.IsNullOrEmpty())
            {
                areThereOperations = true;
                _operationHandler.AddToStored(operations);
            }

            return operations;
        }

        public List<Operation> GetOperationsByType(List<Operation> operations, int definitionId)
        {
            var foundOperations = Operations.GetOperationsByType(operations, definitionId);
            if (!foundOperations.IsNullOrEmpty())
            {
                _operationHandler.AddToStored(foundOperations);
            }

            return foundOperations;
        }

        public List<Operation> GetOperationsForPeriod(Account account, DateTime startDate, DateTime endDate,
            ref bool areThereOperations)
        {
            var foundOperations =
                Operations.GetOperationsForPeriod(account, startDate, endDate);

            if (!foundOperations.IsNullOrEmpty())
            {
                areThereOperations = true;
                _operationHandler.AddToStored(foundOperations);
            }

            return foundOperations;
        }

        #endregion

        #region Register

        public void RegisterDirty(IEntity entity)
        {
            switch (entity)
            {
                case Account account:
                    _accountHandler.RegisterDirty(account);
                    break;
                case Operation operation:
                    _operationHandler.RegisterDirty(operation);
                    break;
            }
        }

        public void RegisterDelete(IEntity entity)
        {
            switch (entity)
            {
                case Account account:
                    _accountHandler.RegisterDelete(account);
                    break;
                case Operation operation:
                    _operationHandler.RegisterDelete(operation);
                    break;
            }
        }

        public void RegisterNew(IEntity entity, ref bool isAdded)
        {
            switch (entity)
            {
                case Account account:
                    _accountHandler.RegisterNew(account, ref isAdded);
                    break;
                case Operation operation:
                    _operationHandler.RegisterNew(operation, ref isAdded);
                    break;
            }
        }

        #endregion

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }

        public void Commit()
        {
            Insert();
            UpdateDirty();
            DeleteRemoved();
        }

        public void Disposing(IEntity entity)
        {
            DisposeNew(entity);
            DisposeDirty(entity);
            DisposeRemoved(entity);
        }

        private void DeleteRemoved()
        {
            _accountHandler.DeleteRemoved();
            _operationHandler.DeleteRemoved();
        }

        private void Insert()
        {
            _accountHandler.Insert();
            _operationHandler.Insert();
        }

        private void UpdateDirty()
        {
            _accountHandler.UpdateDirty();
            _operationHandler.UpdateDirty();
        }

        private void DisposeNew(IEntity entity)
        {
            switch (entity)
            {
                case Account account:
                    _accountHandler.DisposeNew(account);
                    break;
                case Operation operation:
                    _operationHandler.DisposeNew(operation);
                    break;
            }
        }

        private void DisposeDirty(IEntity entity)
        {
            switch (entity)
            {
                case Account account:
                    _accountHandler.DisposeDirty(account);
                    break;
                case Operation operation:
                    _operationHandler.DisposeDirty(operation);
                    break;
            }
        }

        private void DisposeRemoved(IEntity entity)
        {
            switch (entity)
            {
                case Account account:
                    _accountHandler.DisposeRemoved(account);
                    break;
                case Operation operation:
                    _operationHandler.DisposeRemoved(operation);
                    break;
            }
        }
    }
}