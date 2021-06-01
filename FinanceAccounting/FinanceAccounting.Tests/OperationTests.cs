using System;
using System.Collections.Generic;
using System.Linq;
using FinanceAccounting.Controllers;
using FinanceAccounting.Models;
using FinanceAccounting.Models.Request;
using FinanceAccounting.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NUnit.Framework;

namespace FinanceAccounting.Tests.TestRepositories
{
    public class OperationTests
    {
        private TestUnitOfWork _unitOfWork;
        private OperationsController _controller;
        private Account _account;
        private IOperationRepository _operations;

        [SetUp]
        public void Setup()
        {
            var accounts = new TestAccountRepository();
            _operations = new TestOperationRepository();
            _unitOfWork = new TestUnitOfWork(accounts, _operations);
            _controller = new OperationsController(_unitOfWork);
            _account = _unitOfWork.Accounts.Get(1);
        }

        #region GetAccountOperations

        [Test]
        public void GetAccountOperations_AccountId_1_OkResult()
        {
            const int accountID = 1;
            var expectedOperations = _account.Operations;

            var response = _controller.GetAccountOperations(accountID);
            var responseResult = (OkObjectResult) response.Result;
            var responseResultValue = responseResult.Value;

            var expectedCollectionJson = JsonConvert.SerializeObject(expectedOperations);
            var actualCollectionJson = JsonConvert.SerializeObject(responseResultValue);

            Assert.AreEqual(expectedCollectionJson, actualCollectionJson);
            Assert.That(response.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetAccountOperations_AccountId_2_NotFoundResult()
        {
            const int accountID = 2;
            const string expectedResponse = "Операций нет!";

            var response = _controller.GetAccountOperations(accountID);
            var responseResult = (NotFoundObjectResult) response.Result;
            var responseResultValue = responseResult.Value;

            Assert.That(expectedResponse, Is.EqualTo(responseResultValue));
            Assert.That(response.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        [Test]
        public void GetAccountOperations_AccountId_3_NotFoundResult()
        {
            const int accountID = 3;
            const string expectedResponse = "Аккаунта с таким id не найдено!";

            var response = _controller.GetAccountOperations(accountID);
            var responseResult = (NotFoundObjectResult) response.Result;
            var responseResultValue = responseResult.Value;

            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
            Assert.That(response.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        #endregion

        #region GetOperationsByType

        [Test]
        public void GetOperationsByType_AccountId_1_DefinitionId_1_OkResult()
        {
            const int accountID = 1;
            const int definitionID = 1;
            var expectedOperations =
                _account.Operations.Where(o => o.DefinitionId == definitionID);

            var response = _controller.GetOperationsByType(accountID, definitionID);
            var responseResult = (OkObjectResult) response.Result;
            var responseResultValue = responseResult.Value;

            var expectedCollectionJson = JsonConvert.SerializeObject(expectedOperations);
            var actualCollectionJson = JsonConvert.SerializeObject(responseResultValue);

            Assert.That(expectedCollectionJson, Is.EquivalentTo(actualCollectionJson));
            Assert.That(response.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetOperationsByType_AccountId_1_DefinitionId_2_OkResult()
        {
            const int accountID = 1;
            const int definitionID = 2;
            var expectedOperations =
                _account.Operations.Where(o => o.DefinitionId == definitionID);

            var response = _controller.GetOperationsByType(accountID, definitionID);
            var responseResult = (OkObjectResult) response.Result;
            var responseResultValue = responseResult.Value;

            var expectedCollectionJson = JsonConvert.SerializeObject(expectedOperations);
            var actualCollectionJson = JsonConvert.SerializeObject(responseResultValue);

            Assert.That(expectedCollectionJson, Is.EqualTo(actualCollectionJson));
            Assert.That(response.Result, Is.TypeOf<OkObjectResult>());
        }

        [Test]
        public void GetOperationsByType_AccountId_11_DefinitionId_2_NotFoundResult()
        {
            const int accountID = 11;
            const int definitionID = 2;
            const string expectedResponse = "Операций нет!";

            var response = _controller.GetOperationsByType(accountID, definitionID);
            var responseResult = (NotFoundObjectResult) response.Result;
            var responseResultValue = responseResult.Value;

            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
            Assert.That(response.Result, Is.TypeOf<NotFoundObjectResult>());
        }

        #endregion

        #region AddOperation

        [Test]
        public void AddOperation_Operation_AccountID_1_OkResult()
        {
            const int AccountId = 1;
            const string expectedResponse = "Операция(Income) Test Income 456 была добавлена";
            var operationsQuantity = _operations.GetAll().Count;
            var operation = new Operation
            {
                DefinitionId = 1,
                Sum = 456,
                Date = new DateTime(2021, 5, 12),
                Description = "Test Income 456"
            };
            var expectedAccountSum = _account.CurrentSum + operation.Sum;

            var response = _controller.AddOperation(operation, AccountId);
            var responseResult = (OkObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.AreEqual(_account.CurrentSum, expectedAccountSum);
            Assert.That(_operations.GetAll().Count, Is.EqualTo(operationsQuantity + 1));
            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void AddOperation_Operation_AccountID_5_NotFoundResponse()
        {
            const int AccountId = 5;
            const string expectedResponse = "Аккаунт с таким id не найден!";
            var operation = new Operation
            {
                DefinitionId = 1,
                Sum = 456,
                Date = new DateTime(2021, 5, 12),
                Description = "Test Income 456"
            };

            var response = _controller.AddOperation(operation, AccountId);
            var responseResult = (NotFoundObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(response, Is.TypeOf<NotFoundObjectResult>());
            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void AddOperation_Operation_AccountID_1_BadRequest()
        {
            const int AccountId = 1;
            const string expectedResponse = "Не удалось добавить операцию";
            var expectedQuantityOperations = _account.Operations.Count;
            var operation = new Operation
            {
                DefinitionId = 2,
                Sum = 456,
                Date = new DateTime(2021, 5, 12),
                Description = "Test Expense 456"
            };

            var response = _controller.AddOperation(operation, AccountId);
            var responseResult = (BadRequestObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.AreEqual(expectedQuantityOperations, _account.Operations.Count);
            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
        }

        #endregion

        #region GetoperationByDate

        [Test]
        public void GetOperationsByDate_Date_AccountID_1_OKResult()
        {
            const int AccountId = 1;
            const string inputDate = "23/05/2021";
            DateTime date = DateTime.Parse(inputDate);
            var expectedOperations =
                _account.Operations.Where(o => o.Date.Day == date.Date.Day);

            var response = _controller.GetOperationsByDate(inputDate, AccountId);
            var responseResult = (OkObjectResult) response;
            var responseResultValue = responseResult.Value as List<Operation>;

            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.That(expectedOperations, Is.EquivalentTo(responseResultValue));
        }

        [Test]
        public void GetOperationsByDate_Date_AccountID_1_NotFoundResult()
        {
            const int AccountId = 1;
            var date = "30/5/2021";
            const string expectedResponse = "За данное число операций нет";

            var response = _controller.GetOperationsByDate(date, AccountId);
            var responseResult = (NotFoundObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(response, Is.TypeOf<NotFoundObjectResult>());
            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void GetOperationsByDate_Date_AccountID_66_NotFoundResult()
        {
            const int AccountId = 66;
            var date = "30/05/2021";
            const string expectedResponse = "Аккаунт с таким id не найден!";

            var response = _controller.GetOperationsByDate(date, AccountId);
            var responseResult = (NotFoundObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(response, Is.TypeOf<NotFoundObjectResult>());
            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void GetOperationsByDate_IncorrectDateFormat_AccountID_1_NotFoundResult()
        {
            const int AccountId = 66;
            var date = "30/55/2021";
            const string expectedResponse = "Не верный формат даты!";

            var response = _controller.GetOperationsByDate(date, AccountId);
            var responseResult = (BadRequestObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(response, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
        }

        #endregion

        #region GetOperationsForPeriod

        [Test]
        public void GetOperationsForPeriod_StartDate_EndDate_AccountID_1_OKResult()
        {
            const int AccountId = 1;

            var dateRange = new DateRange
            {
                StartDate = "22/05/2021",
                EndDate = "23/05/2021"
            };

            DateTime startDate = DateTime.Parse(dateRange.StartDate);
            DateTime endDate = DateTime.Parse(dateRange.EndDate);

            var expectedOperations =
                _account.Operations
                    .Where(o => o.Date.Day >= startDate.Date.Day && o.Date.Day <= endDate.Date.Day).ToList();

            var response = _controller.GetOperationsForPeriod(AccountId, dateRange);
            var responseResult = (OkObjectResult) response;
            var responseResultValue = responseResult.Value as List<Operation>;

            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.That(expectedOperations, Is.EquivalentTo(responseResultValue));
        }

        [Test]
        public void GetOperationsForPeriod_StartDate_EndDate_AccountID_1_NotFoundResult()
        {
            const int AccountId = 1;
            const string expectedResponse = "За данный период операций нет";

            var dateRange = new DateRange
            {
                StartDate = "26/05/2021",
                EndDate = "27/05/2021"
            };

            var response = _controller.GetOperationsForPeriod(AccountId, dateRange);
            var responseResult = (NotFoundObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(response, Is.TypeOf<NotFoundObjectResult>());
            Assert.AreEqual(expectedResponse, responseResultValue);
        }

        [Test]
        public void GetOperationsForPeriod_StartDate_EndDate_AccountID_66_NotFoundResult()
        {
            const int AccountId = 66;
            const string expectedResponse = "Аккаунт с таким id не найден!";

            var dateRange = new DateRange
            {
                StartDate = "26/05/2021",
                EndDate = "27/05/2021"
            };

            var response = _controller.GetOperationsForPeriod(AccountId, dateRange);
            var responseResult = (NotFoundObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(response, Is.TypeOf<NotFoundObjectResult>());
            Assert.AreEqual(expectedResponse, responseResultValue);
        }

        #endregion

        #region UpdateOperation

        [Test]
        public void UpdateOperation_Operation_OKResult()
        {
            var operation = new Operation
            {
                OperationId = 1,
                DefinitionId = 2,
                Sum = 155,
                Date = new DateTime(2021, 5, 10),
                Description = "Test update"
            };
            var foundOperations = _operations.Get(operation.OperationId);

            var response = _controller.UpdateOperation(operation);
            var responseResult = (OkObjectResult) response;
            var responseResultValue = responseResult.Value as Operation;

            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.AreEqual(foundOperations.OperationId, responseResultValue.OperationId);
            Assert.AreEqual(foundOperations.Sum, responseResultValue.Sum);
            Assert.AreEqual(foundOperations.DefinitionId, responseResultValue.DefinitionId);
            Assert.AreEqual(foundOperations.Date, responseResultValue.Date);
            Assert.AreEqual(foundOperations.Description, responseResultValue.Description);
            Assert.AreEqual(foundOperations.AccountId, responseResultValue.AccountId);
            Assert.AreEqual(foundOperations.Account, responseResultValue.Account);
            Assert.AreEqual(foundOperations.PurposeOperation, responseResultValue.PurposeOperation);
        }

        [Test]
        public void UpdateOperation_Operation_NotFoundResult()
        {
            const string expectedResponse = "Операция для изменения не найдена";
            var operation = new Operation
            {
                OperationId = 66,
                DefinitionId = 2,
                Sum = 155,
                Date = new DateTime(2021, 5, 10),
                Description = "Test update"
            };

            var response = _controller.UpdateOperation(operation);
            var responseResult = (NotFoundObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(response, Is.TypeOf<NotFoundObjectResult>());
            Assert.AreEqual(expectedResponse, responseResultValue);
        }

        #endregion

        #region DeleteOperation

        [Test]
        public void DeleteOperation_OperationID_8_OkResult()
        {
            const int OperationID = 8;
            const string expectedResponse = "Операция удалена";
            var operations = _unitOfWork.Operations.GetAll();
            var operationsQuantity = operations.Count;

            var response = _controller.DeleteOperation(OperationID);
            var responseResult = (OkObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(operations.Count, Is.EqualTo(operationsQuantity - 1));
            Assert.That(response, Is.TypeOf<OkObjectResult>());
            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
        }

        [Test]
        public void DeleteOperation_OperationID_66_NotFoundResult()
        {
            const int OperationID = 66;
            const string expectedResponse = "Операция с таким ID не найдена";

            var response = _controller.DeleteOperation(OperationID);
            var responseResult = (NotFoundObjectResult) response;
            var responseResultValue = responseResult.Value;

            Assert.That(response, Is.TypeOf<NotFoundObjectResult>());
            Assert.That(responseResultValue, Is.EqualTo(expectedResponse));
        }

        #endregion
    }
}