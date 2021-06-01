using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Observer.Interfaces
{
    public class OperationHandle : IHandleEntities
    {
        private readonly IOperationRepository _operations;
        private readonly IAccountRepository _accounts;

        private readonly List<Operation> _newOperations = new List<Operation>();
        private readonly List<Operation> _dirtyOperations = new List<Operation>();
        private readonly List<Operation> _removeOperations = new List<Operation>();
        private readonly List<Operation> _storedOperations = new List<Operation>();

        public OperationHandle(IOperationRepository operations, IAccountRepository accounts)
        {
            _operations = operations;
            _accounts = accounts;
        }

        #region handle_clear

        public void AddToStoredOperations(List<Operation> operations)
        {
            foreach (var operation in operations)
            {
                AddToStoredOperations(operation);
            }
        }

        #endregion

        #region handle_delete

        public void RegisterDelete(Operation operation)
        {
            var isContainsRemoved = _removeOperations.Contains(operation);
            bool removedFromNew = _newOperations.Remove(operation);

            if (!removedFromNew)
            {
                _dirtyOperations.Remove(operation);
                if (!IsNull(operation) && !isContainsRemoved)
                {
                    _removeOperations.Add(operation);
                }
            }
        }

        public void DeleteRemoved()
        {
            if (!_removeOperations.IsNullOrEmpty())
            {
                foreach (var operation in _removeOperations)
                {
                    _operations.Delete(operation);
                    RemoveFromStoredOperations(operation);
                }

                _removeOperations.Clear();
                _operations.Save();
            }
        }

        #endregion

        #region handle_dirty

        public void RegisterDirty(Operation operation)
        {
            if (operation.OperationId != 0 && operation.OperationId != null && !_newOperations.Contains(operation) &&
                !_dirtyOperations.Contains(operation))
            {
                _dirtyOperations.Add(operation);
            }
        }

        public void UpdateDirty()
        {
            if (!_dirtyOperations.IsNullOrEmpty())
            {
                foreach (var operation in _dirtyOperations)
                {
                    _operations.Update(operation);
                    UpdateStoredOperations(_dirtyOperations);
                }

                _dirtyOperations.Clear();
                _operations.Save();
            }
        }

        #endregion

        #region handle_new

        public void RegisterNew(Operation operation, ref bool isAdded)
        {
            var isContainNew = _newOperations.Contains(operation);
            var isContainDirty = _dirtyOperations.Contains(operation);

            AccountAmountCalculation(operation, ref isAdded);

            if (!IsNull(operation) && !isContainNew && !isContainDirty)
            {
                _newOperations.Add(operation);
            }
        }

        public void Insert()
        {
            if (!_newOperations.IsNullOrEmpty())
            {
                foreach (var operation in _newOperations)
                {
                    _operations.Add(operation);
                    AddToStoredOperations(operation);
                }

                _newOperations.Clear();
                _operations.Save();
                _accounts.Save();
            }
        }

        #endregion

        #region handle_get

        public Operation GetById(int id)
        {
            return _storedOperations.FirstOrDefault(o => o.OperationId == id);
        }

        #endregion

        #region handle_dispose

        public void DisposeNew(Operation operation)
        {
            Disposing(_newOperations, operation);
        }

        public void DisposeRemoved(Operation operation)
        {
            Disposing(_removeOperations, operation);
        }

        public void DisposeDirty(Operation operation)
        {
            Disposing(_dirtyOperations, operation);
        }

        #endregion

        public void AddToStoredOperations(Operation operation)
        {
            var isContainsDirty = _dirtyOperations.Contains(operation);
            var isContainsRemoved = _removeOperations.Contains(operation);
            var isContainsClear = _storedOperations.Contains(operation);

            if (!IsNull(operation) && !isContainsRemoved && !isContainsDirty && !isContainsClear)
            {
                _storedOperations.Add(operation);
            }
        }

        private void RemoveFromStoredOperations(Operation operation)
        {
            if (!_storedOperations.IsNullOrEmpty())
            {
                var operationForRemove = _storedOperations.FirstOrDefault(o => o.OperationId == operation.OperationId);
                if (operationForRemove != null)
                {
                    _storedOperations.Remove(operationForRemove);
                }
            }
        }

        private void UpdateStoredOperations(List<Operation> operations)
        {
            if (_storedOperations.IsNullOrEmpty() || operations.IsNullOrEmpty()) return;
            foreach (var operation in operations)
            {
                var storedOperation =
                    _storedOperations.FirstOrDefault(o => o.OperationId == operation.OperationId);

                if (operation.Sum > 0)
                {
                    storedOperation.Sum = operation.Sum;
                }

                if (operation.DefinitionId == (int) OperationEnum.Income ||
                    operation.DefinitionId == (int) OperationEnum.Expense)
                {
                    storedOperation.DefinitionId = operation.DefinitionId;
                }

                if (operation.Date != null && operation.Date != DateTime.MinValue)
                {
                    storedOperation.Date = operation.Date;
                }

                if (operation.Description != null)
                {
                    storedOperation.Description = operation.Description;
                }
            }
        }

        private void AccountAmountCalculation(Operation operation, ref bool isAdded)
        {
            var account = _accounts.Get(operation.AccountId);
            switch (operation.PurposeOperation)
            {
                case OperationEnum.Income:
                    account.CurrentSum += operation.Sum;
                    isAdded = true;
                    break;

                case OperationEnum.Expense when (account.CurrentSum - operation.Sum) >= 0:
                    account.CurrentSum -= operation.Sum;
                    isAdded = true;
                    break;
            }
        }

        private static bool IsNull(Operation operation)
        {
            return operation == null;
        }

        private static void Disposing(List<Operation> collection, Operation operation)
        {
            if (!IsNull(operation) && collection.Contains(operation))
            {
                collection.Remove(operation);
            }
        }
    }
}