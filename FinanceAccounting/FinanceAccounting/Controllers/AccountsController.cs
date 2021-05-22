using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceAccounting.DataBase;
using FinanceAccounting.Models;
using FinanceAccounting.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
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
            try
            {
                _unitOfWork.Accounts.Create(account);
                return Ok("Аккаунт был добавлен");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Ну удалось добавить аккаунт!");
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateAccount(int id, [FromBody] Account account)
        {
            var findAccount = _unitOfWork.Accounts.Get(id);
            if (findAccount != null)
            {
                try
                {
                    _unitOfWork.Accounts.Update(account);
                    return Ok(account);
                }
                catch
                {
                    return BadRequest("Не удалось изменить аккаунт!");
                }
            }

            return NotFound("Аккаунт с таким id не найден!");
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