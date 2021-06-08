using System;
using Castle.Core.Internal;
using FinanceAccounting.Models;
using FinanceAccounting.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            var accounts = _unitOfWork.GetAllAccounts();

            if (!accounts.IsNullOrEmpty())
            {
                return Ok(accounts);
            }

            return NotFound("Аккаунтов нет");
        }

        [HttpGet("{id:int}")]
        public IActionResult GetAccount(int id)
        {
            var account = _unitOfWork.GetAccountById(id);

            if (account != null)
            {
                return Ok(account);
            }

            return NotFound("Аккаунт с таким id не найден!");
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            var isAdded = false;
            if (account == null)
            {
                return BadRequest("Нет аккаунта для добавления!");
            }

            try
            {
                _unitOfWork.RegisterNew(account, ref isAdded);
                _unitOfWork.Commit();
                return Ok("Аккаунт был добавлен");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _unitOfWork.Disposing(account);
                return BadRequest("Ну удалось добавить аккаунт!");
            }
        }

        [HttpPut]
        public IActionResult UpdateAccount([FromBody] Account transmittedAccount)
        {
            var foundAccount = _unitOfWork.GetAccountById(transmittedAccount.AccountId);

            if (foundAccount == null)
            {
                return NotFound("Аккаунт для изменения не найден");
            }

            try
            {
                _unitOfWork.RegisterDirty(transmittedAccount);
                _unitOfWork.Commit();
                return Ok(foundAccount);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _unitOfWork.Disposing(transmittedAccount);
                return BadRequest("Не получилось изменить аккаунт");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var account = _unitOfWork.GetAccountById(id);

            if (account != null)
            {
                try
                {
                    _unitOfWork.RegisterDelete(account);
                    _unitOfWork.Commit();
                    return Ok("Аккаунт удалён");
                }
                catch
                {
                    _unitOfWork.Disposing(account);
                    return BadRequest("Не удалось удалить аккаунт!");
                }
            }

            return NotFound("Аккаунт с таким id не найден!");
        }
    }
}