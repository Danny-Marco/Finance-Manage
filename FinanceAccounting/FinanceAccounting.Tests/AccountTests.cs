using System.Collections.Generic;
using FinanceAccounting.Controllers;
using FinanceAccounting.Models;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;

namespace FinanceAccounting.Tests
{
    public class AccountTests
    {
        private TestUnitOfWork _unitOfWork;
        private AccountsController _controller;
        private IActionResult _response;
        private Account _account;
        
        [SetUp]
        public void Setup()
        {
            _unitOfWork = new TestUnitOfWork();
            _controller = new AccountsController(_unitOfWork);
        }

        [Test]
        public void Should_Return_OkResult()
        {
            _response = _controller.GetAllAccounts();

            Assert.That(_response, Is.TypeOf<OkObjectResult>());
        }
        
        [Test]
        public void GetAccount_1_ShouldReturn_OkResult()
        {
            var accountId = 1;
            
            _response = _controller.GetAccount(accountId);
            
            Assert.That(_response, Is.InstanceOf<OkObjectResult>());
        }
        
        [Test]
        public void GetAccount_2_ShouldReturn_NotFoundResult()
        {
            var accountId = 33;
            
            _response = _controller.GetAccount(accountId);
            
            Assert.That(_response, Is.InstanceOf<NotFoundObjectResult>());
        }
        
        [Test]
        public void CreateAccount_Account_OkResult()
        {
            _account = new Account
            {
                AccountId = 3,
                CurrentSum = 333,
                Operations = new List<Operation>()
            };

            var accountQuantity = _unitOfWork.Accounts.Count();

            _response = _controller.CreateAccount(_account);
            
            Assert.That(_response, Is.TypeOf<OkObjectResult>());
            Assert.That(_unitOfWork.Accounts.Count(), Is.EqualTo(accountQuantity + 1));
        }
        
        [Test]
        public void CreateAccount_Account_BadRequest()
        {
            _account = null;
            var quantityAccount = _unitOfWork.Accounts.Count();

            _response = _controller.CreateAccount(_account);
            
            Assert.That(_response, Is.TypeOf<BadRequestObjectResult>());
            Assert.That(_unitOfWork.Accounts.Count(), Is.Not.EqualTo(quantityAccount + 1));
        }
        
        [Test]
        public void DeleteAccount_Account_BadRequest()
        {
            var accountId = 3;
            var accountsQuantity = _unitOfWork.Accounts.Count();

            _response = _controller.DeleteAccount(accountId);
            
            Assert.That(_response, Is.TypeOf<NotFoundObjectResult>());
            Assert.That(_unitOfWork.Accounts.Count(), Is.Not.EqualTo(accountsQuantity - 1));
        }
        
        [Test]
        public void DeleteAccount_Account_OkResult()
        {
            var accountId = 2;
            var accountsQuantity = _unitOfWork.Accounts.Count();

            _response = _controller.DeleteAccount(accountId);
            
            Assert.That(_response, Is.TypeOf<OkObjectResult>());
            Assert.That(_unitOfWork.Accounts.Count(), Is.EqualTo(accountsQuantity - 1));
        }
    }
}