using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinanceAccounting.Models;
using FinanceAccounting.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public OperationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                var operations = _unitOfWork.Operations.GetAll();

                return Ok(operations);
            }
            catch
            {
                return BadRequest("Аккаунтов нет");
            }
        }
        

        
        [HttpGet("{id:int}", Name = "GetOperations")]
        public IActionResult Get(int id)
        {
            var account = _unitOfWork.Accounts.Get(id);

            if (account == null)
            {
                return BadRequest("Аккаунт с таким id не найден!");
            }

            var operations = account.Operations;

            if (operations != null)
            {
                return Ok(operations);
            }

            return BadRequest("Операций нет");
        }


        [HttpPost("{id:int}")]
        public IActionResult AddOperation([FromBody] Operation operation, int id)
        {
            var account = _unitOfWork.Accounts.Get(id);

            if (account == null)
            {
                return BadRequest("Аккаунт с таким id не найден!");
            }

            try
            {
                _unitOfWork.Operations.CreateOperation(id, operation);
                return Ok($"Операция {operation.PurposeOperation} : {operation.Description} была добавлена");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Не удалось добавить операцию");
            }
        }

        // PUT: api/Operations/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _unitOfWork.Operations.Delete(id);
                return Ok("Операция удалена");
            }
            catch (Exception e)
            {
                return BadRequest("Не получилось удалить операцию");
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}