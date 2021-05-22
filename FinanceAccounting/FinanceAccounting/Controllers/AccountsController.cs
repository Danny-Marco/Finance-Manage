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
        public IActionResult Get()
        {
            try
            {
                var accounts = _unitOfWork.Accounts.GetAllAccounts();

                return Ok(accounts);
            }
            catch
            {
                return BadRequest("Аккаунтов нет");
            }
        }

        
        [HttpGet("{id}", Name = "GetOperations")]
        public async Task<IActionResult> Get(int id)
        {
            // var account = await _DB.Accounts.FindAsync(id);
            //
            // if (account == null)
            // {
            //     return BadRequest("Аккаунт с таким id не найден!");
            // }
            //
            // var operations = account.Operations.ToList();
            //
            // if (operations != null)
            // {
            //     return Ok(operations);
            // }
            //
            return BadRequest("Аккаунтов нет");
        }

        // POST: api/Accounts
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Account account)
        {
            // if (account!= null)
            // {
            //     _DB.Accounts.Add(account);
            //     await _DB.SaveChangesAsync();
            //     return Ok("Аккаунт был добавлен");
            // }

            return BadRequest("Ну удалось добавить аккаунт!");
        }
        
        [HttpPost("{id:int}")]
        public async Task<IActionResult> Post([FromBody] Operation operation, int id)
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
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // var account = await _DB.Accounts.FindAsync(id);
            // if (account != null)
            // {
            //     _DB.Accounts.Remove(account);
            //     return Ok("Аккаунт был удалён");
            // }

            return BadRequest("Аккаунт с таким ID не найден!");
        }
    }
}
