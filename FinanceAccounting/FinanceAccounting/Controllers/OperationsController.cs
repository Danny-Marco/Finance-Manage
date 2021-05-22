using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using FinanceAccounting.Models;
using FinanceAccounting.Repositories.Interfaces;
using FinanceAccounting.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private IUnitOfWork _unitOfWork;

        public OperationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public IActionResult GetAllOperations()
        {
            try
            {
                var operations = _unitOfWork.Operations.GetAll();

                return Ok(operations);
            }
            catch
            {
                return NotFound("Операций нет");
            }
        }


        [HttpGet("{id:int}")]
        public IActionResult GetAccountOperations(int id)
        {
            var account = _unitOfWork.Operations.GetAccount(id);

            if (account == null)
            {
                return NotFound("Операции с таким id не найдено!");
            }

            if (account.Operations.IsNullOrEmpty())
            {
                return NotFound("Операций нет!");
            }

            try
            {
                var response = _unitOfWork.Operations.GetAccountOperations(account);

                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Что то пошло не так!");
            }
        }


        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddOperation([FromBody] Operation operation, int id)
        {
            var account = _unitOfWork.Accounts.Get(id);

            if (account == null)
            {
                return NotFound("Аккаунт с таким id не найден!");
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

        [HttpPost]
        [ProducesResponseType(typeof(List<Operation>), 200)]
        public IActionResult GetAccountOperationsByDate([FromBody] DateTime date, int id)
        {
            var account = _unitOfWork.Accounts.Get(id);

            if (account == null)
            {
                return NotFound("Аккаунт с таким id не найден!");
            }

            try
            {
                var operations = _unitOfWork.Operations.GetOperationsByDate(account, date);

                return Ok(operations);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NotFound("За данное число операций нет");
            }
        }
        
        
        [HttpPut]
        public IActionResult UpdateOperation([FromBody] Operation operation)
        {
            try
            {
                _unitOfWork.Operations.Update(operation);
                return Ok(operation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Не получилось изменить операцию");
                throw;
            }
            
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOperation(int id)
        {
            var operation = _unitOfWork.Operations.Get(id);
            if (operation == null)
            {
                return BadRequest("Операция с таким ID не найдена");
            }
            
            try
            {
                _unitOfWork.Operations.Delete(operation);
                return Ok("Операция удалена");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Не получилось удалить операцию");
            }
        }
    }
}