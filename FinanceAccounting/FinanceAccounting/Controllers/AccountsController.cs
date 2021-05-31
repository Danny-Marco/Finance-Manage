using System;using FinanceAccounting.Models;
using FinanceAccounting.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAllAccounts()
        {
            try
            {
                var accounts = _unitOfWork.Accounts.GetAll();

                return Ok(accounts);
            }
            catch
            {
                return NotFound("Аккаунтов нет");
            }
        }


        [HttpGet("{id:int}")]
        public IActionResult GetAccount(int id)
        {
            var account = _unitOfWork.Accounts.Get(id);

            if (account != null)
            {
                return Ok(account);
            }

            return NotFound("Аккаунт с таким id не найден!");
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] Account account)
        {
            if (account == null)
            {
                return BadRequest("Нет аккаунта для добавления!");
            }
            
            try
            {
                _unitOfWork.Accounts.Create(account);
                _unitOfWork.Save();
                return Ok("Аккаунт был добавлен");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Ну удалось добавить аккаунт!");
            }
        }
        
        [HttpDelete("{id}")]
        public IActionResult DeleteAccount(int id)
        {
            var account = _unitOfWork.Accounts.Get(id);

            if (account != null)
            {
                try
                {
                    _unitOfWork.Accounts.Delete(account);
                    _unitOfWork.Save();
                    return Ok("Аккаунт удалён");
                }
                catch
                {
                    return BadRequest("Не удалось удалить аккаунт!");
                }
            }

            return NotFound("Аккаунт с таким id не найден!");
        }
    }
}