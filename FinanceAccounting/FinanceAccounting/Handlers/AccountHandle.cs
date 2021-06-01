using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using FinanceAccounting.Models;
using FinanceAccounting.Observer.Interfaces;
using FinanceAccounting.Repositories.Interfaces;

namespace FinanceAccounting.Observer
{
    public class AccountHandle : IHandleEntities
    {
        private readonly List<Account> _newAccounts = new List<Account>();
        private readonly List<Account> _dirtyAccounts = new List<Account>();
        private readonly List<Account> _removedAccounts = new List<Account>();
        private readonly List<Account> _storedAccounts = new List<Account>();

        private readonly IAccountRepository _accounts;

        public AccountHandle(IAccountRepository accounts)
        {
            _accounts = accounts;
        }

        #region handle_new

        public void RegisterNew(Account account, ref bool isAdded)
        {
            var isContainsNew = _newAccounts.Contains(account);
            var isContainsDirty = _dirtyAccounts.Contains(account);

            if (!IsNull(account) && !isContainsNew && !isContainsDirty)
            {
                _newAccounts.Add(account);
            }
        }

        public void AddToStoredAccounts(Account account)
        {
            var isContainsStored = _storedAccounts.Contains(account);
            var isContainsDirty = _dirtyAccounts.Contains(account);
            var isContainsRemoved = _removedAccounts.Contains(account);

            if (!IsNull(account) && !isContainsStored && !isContainsDirty && !isContainsRemoved)
            {
                _storedAccounts.Add(account);
            }
        }

        public void AddToStoredAccounts(List<Account> accounts)
        {
            foreach (var account in accounts)
            {
                AddToStoredAccounts(account);
            }
        }

        public void Insert()
        {
            if (!_newAccounts.IsNullOrEmpty())
            {
                foreach (var account in _newAccounts)
                {
                    _accounts.Add(account);
                    AddToStoredAccounts(account);
                }

                _newAccounts.Clear();
                _accounts.Save();
            }
        }

        #endregion

        #region handle_delete

        public void RegisterDelete(Account account)
        {
            bool removedFromNewAccount = _newAccounts.Remove(account);
            if (removedFromNewAccount) return;
            _dirtyAccounts.Remove(account);
            if (account.AccountId != 0 && account.AccountId != null && !_removedAccounts.Contains(account))
            {
                _removedAccounts.Add(account);
            }
        }

        public void DeleteRemoved()
        {
            if (!_removedAccounts.IsNullOrEmpty())
            {
                foreach (var account in _removedAccounts)
                {
                    _accounts.Delete(account);
                    RemoveFromStoredAccounts(account);
                }

                _removedAccounts.Clear();
                _accounts.Save();
            }
        }

        #endregion

        #region handle_dirty

        public void RegisterDirty(Account account)
        {
            if (account.AccountId != 0 && account.AccountId != null && !_newAccounts.Contains(account) &&
                !_dirtyAccounts.Contains(account))
            {
                _dirtyAccounts.Add(account);
            }
        }

        public void UpdateDirty()
        {
            if (!_dirtyAccounts.IsNullOrEmpty())
            {
                foreach (var account in _dirtyAccounts)
                {
                    _accounts.Update(account);
                    UpdateStoredAccounts(_dirtyAccounts);
                }

                _dirtyAccounts.Clear();
                _accounts.Save();
            }
        }

        #endregion

        #region handle_get

        public Account GetById(int id)
        {
            return _storedAccounts.Find(a => a.AccountId == id);
        }

        #endregion

        #region handle_dispose

        public void DisposeNew(Account account)
        {
            Disposing(_newAccounts, account);
        }

        public void DisposeRemoved(Account account)
        {
            Disposing(_removedAccounts, account);
        }

        public void DisposeDirty(Account account)
        {
            Disposing(_dirtyAccounts, account);
        }

        #endregion

        private void RemoveFromStoredAccounts(Account account)
        {
            if (!_storedAccounts.IsNullOrEmpty())
            {
                var elementForRemove =
                    _storedAccounts.FirstOrDefault(a => a.AccountId == account.AccountId);

                if (elementForRemove != null)
                {
                    _storedAccounts.Remove(elementForRemove);
                }
            }
        }

        private void UpdateStoredAccounts(List<Account> collection)
        {
            if (_storedAccounts.IsNullOrEmpty() || collection.IsNullOrEmpty()) return;
            foreach (var account in collection)
            {
                var storedAccount = _storedAccounts.FirstOrDefault(a => a.AccountId == account.AccountId);
                if (storedAccount != null)
                {
                    storedAccount.CurrentSum = account.CurrentSum;
                }
            }
        }

        private static void Disposing(List<Account> collection, Account account)
        {
            if (!IsNull(account) && collection.Contains(account))
            {
                collection.Remove(account);
            }
        }

        private static bool IsNull(Account account)
        {
            return account == null;
        }
    }
}