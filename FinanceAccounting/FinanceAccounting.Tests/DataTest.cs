using System;
using System.Collections.Generic;
using FinanceAccounting.Models;

namespace FinanceAccounting.Tests
{
    public class DataTest
    {
        public List<Account> CreateAccounts()
        {
            var accounts = new List<Account>
            {
                CreateAccountWithOperations(), CreateAccountWithoutOperations(), CreateWithoutExpense()
            };

            return accounts;
        }

        public List<Operation> CreateOperations()
        {
            var operations = new List<Operation>();

            var account1 = CreateAccountWithOperations();
            var account2 = CreateWithoutExpense();

            foreach (var operation in account1.Operations)
            {
                operations.Add(operation);
            }

            foreach (var operation in account2.Operations)
            {
                operations.Add(operation);
            }

            return operations;
        }

        private Account CreateAccountWithOperations()
        {
            var account = new Account
            {
                AccountId = 1,
                CurrentSum = 111,
                Operations = new List<Operation>()
            };

            var operation1 = new Operation
            {
                OperationId = 1,
                DefinitionId = 1,
                Date = new DateTime(2021, 5, 20),
                Sum = 1_000,
                Description = "Salary",
                AccountId = 1,
                Account = account
            };

            var operation2 = new Operation
            {
                OperationId = 2,
                DefinitionId = 1,
                Date = new DateTime(2021, 5, 21),
                Sum = 2_000,
                Description = "Win the lottery",
                AccountId = 1,
                Account = account
            };

            var operation3 = new Operation
            {
                OperationId = 3,
                DefinitionId = 2,
                Date = new DateTime(2021, 5, 22, 00, 24, 51),
                Sum = 33,
                Description = "Shopping",
                AccountId = 1,
                Account = account
            };

            var operation4 = new Operation
            {
                OperationId = 4,
                DefinitionId = 2,
                Date = new DateTime(2021, 5, 23, 00, 24, 55),
                Sum = 44,
                Description = "Buying something",
                AccountId = 1,
                Account = account
            };

            var operation5 = new Operation
            {
                OperationId = 5,
                DefinitionId = 2,
                Date = new DateTime(2021, 5, 23, 00, 24, 57),
                Sum = 55,
                Description = "Buying food",
                AccountId = 1,
                Account = account
            };

            var operation6 = new Operation
            {
                OperationId = 6,
                DefinitionId = 2,
                Date = new DateTime(2021, 5, 23, 00, 25, 30),
                Sum = 66,
                Description = "Buying food",
                AccountId = 1,
                Account = account
            };

            var operation7 = new Operation
            {
                OperationId = 7,
                DefinitionId = 2,
                Date = new DateTime(2021, 5, 23, 00, 24, 40),
                Sum = 77,
                Description = "Payment of fines",
                AccountId = 1,
                Account = account
            };

            var operation8 = new Operation
            {
                OperationId = 8,
                DefinitionId = 2,
                Date = new DateTime(2021, 5, 23, 00, 24, 55),
                Sum = 88,
                Description = "Got a debt",
                AccountId = 1,
                Account = account
            };

            account.Operations.Add(operation1);
            account.Operations.Add(operation2);
            account.Operations.Add(operation3);
            account.Operations.Add(operation4);
            account.Operations.Add(operation5);
            account.Operations.Add(operation6);
            account.Operations.Add(operation7);
            account.Operations.Add(operation8);

            return account;
        }

        private Account CreateAccountWithoutOperations()
        {
            var accountWithoutOperations = new Account
            {
                AccountId = 2,
                CurrentSum = 222,
                Operations = new List<Operation>()
            };

            return accountWithoutOperations;
        }

        private Account CreateWithoutExpense()
        {
            var accountWithoutExpense = new Account
            {
                AccountId = 11,
                CurrentSum = 333,
                Operations = new List<Operation>()
            };

            var operation1 = new Operation
            {
                OperationId = 1,
                DefinitionId = 1,
                Date = new DateTime(2021, 5, 21),
                Sum = 1_000,
                Description = "Salary",
                AccountId = 1,
                Account = accountWithoutExpense
            };

            var operation2 = new Operation
            {
                OperationId = 2,
                DefinitionId = 1,
                Date = new DateTime(2021, 5, 22),
                Sum = 2_000,
                Description = "Win the lottery",
                AccountId = 1,
                Account = accountWithoutExpense
            };

            accountWithoutExpense.Operations.Add(operation1);
            accountWithoutExpense.Operations.Add(operation2);

            return accountWithoutExpense;
        }
    }
}