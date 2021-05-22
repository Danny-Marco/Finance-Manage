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
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;
        
        public AccountsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var accounts = _unitOfWork.Accounts.GetAll();

                return Ok(accounts);
            }
            catch
            {
                return BadRequest("Аккаунтов нет");
            }
        }

        
        [HttpGet("{id:int}")]
        public IActionResult GetAccount(int id)
        {
            var account = _unitOfWork.Accounts.Get(id);
            
            // if (account == null)
            // {
            //     return BadRequest("Аккаунт с таким id не найден!");
            // }
            //
            // var operations = account.Operations;
            //
            // if (operations != null)
            // {
            //     return Ok(operations);
            // }
            if (account != null)
            {
                return Ok(account);
            }
            
            return BadRequest("Аккаунтов нет");
        }

        [HttpPost]
        public IActionResult Post([FromBody] Account account)
        {
            if (account!= null)
            {
                _unitOfWork.Accounts.Create(account);
                return Ok("Аккаунт был добавлен");
            }

            return BadRequest("Ну удалось добавить аккаунт!");
        }
        
        [HttpPost("{id:int}")]
        public IActionResult Post([FromBody] Operation operation, int id)
        {
            // var account = await _DB.Accounts.FindAsync(id);
            // if (account!= null)
            // {
            //     account.Operations.Add(operation);
            //     await _DB.SaveChangesAsync();
            //     
            //     return Ok("Операция была добавлена");
            // }

            return BadRequest("Ну удалось добавить операцию!");
        }

        // PUT: api/Accounts/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody] Account account)
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

            return BadRequest("Аккаунт с таким ID не найден!");
        }

        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var account = _unitOfWork.Accounts.Get(id);
            if (account != null)
            {
                try
                {
                    _unitOfWork.Accounts.Delete(account.AccountId);
                    return Ok("Аккаунт удалён");
                }
                catch
                {
                    return BadRequest("Не удалось удалить аккаунт!");
                }
                
            }

            return BadRequest("Аккаунт с таким ID не найден!");
        }
    }
}
