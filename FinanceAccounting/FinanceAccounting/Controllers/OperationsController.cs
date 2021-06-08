using System;
using System.Collections.Generic;
using Castle.Core.Internal;
using FinanceAccounting.Models;
using FinanceAccounting.Models.Request;
using FinanceAccounting.UnitsOfWork.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FinanceAccounting.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OperationsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OperationsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("{id:int}")]
        public ActionResult<List<Operation>> GetAccountOperations(int id)
        {
            var account = _unitOfWork.GetAccountById(id);

            if (account == null)
            {
                return NotFound("Аккаунта с таким id не найдено!");
            }

            if (account.Operations.IsNullOrEmpty())
            {
                return NotFound("Операций нет!");
            }

            try
            {
                var response = account.Operations;
                return Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest("Что то пошло не так!");
            }
        }

        [HttpGet("{id:int}")]
        public ActionResult<List<Operation>> GetOperationsByType(int id, int definitionId)
        {
            var account = _unitOfWork.GetAccountById(id);

            if (account == null)
            {
                return NotFound("Аккаунта с таким id не найдено!");
            }

            if (account.Operations.IsNullOrEmpty())
            {
                return NotFound("Операций нет!");
            }

            try
            {
                var response =
                    _unitOfWork.GetOperationsByType(account.Operations, definitionId);

                if (!response.IsNullOrEmpty())
                {
                    return Ok(response);
                }

                return NotFound("Операций нет!");
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
        [ProducesResponseType(404)]
        public IActionResult AddOperation([FromBody] Operation operation, int id)
        {
            var isAdded = false;
            var account = _unitOfWork.GetAccountById(id);

            if (account == null)
            {
                return NotFound("Аккаунт с таким id не найден!");
            }

            operation.AccountId = id;
            _unitOfWork.RegisterNew(operation, ref isAdded);

            if (isAdded)
            {
                _unitOfWork.Commit();
                return Ok($"Операция({operation.PurposeOperation}) {operation.Description} была добавлена");
            }

            _unitOfWork.Disposing(operation);
            return BadRequest("Не удалось добавить операцию");
        }

        [HttpPost]
        [ProducesResponseType(typeof(List<Operation>), 200)]
        public IActionResult GetOperationsByDate([FromBody] string inputDate, int id)
        {
            var areThereOperations = false;

            if (!DateTime.TryParse(inputDate, out var date))
            {
                return BadRequest("Не верный формат даты!");
            }

            var account = _unitOfWork.GetAccountById(id);

            if (account == null)
            {
                return NotFound("Аккаунт с таким id не найден!");
            }

            var operations =
                _unitOfWork.GetOperationsByDate(account, date, ref areThereOperations);

            if (areThereOperations)
            {
                return Ok(operations);
            }

            return NotFound("За данное число операций нет");
        }

        [HttpPost]
        public IActionResult GetOperationsForPeriod(int id, [FromBody] DateRange dateRange)
        {
            var areThereOperations = false;

            var account = _unitOfWork.GetAccountById(id);

            if (account == null)
            {
                return NotFound("Аккаунт с таким id не найден!");
            }

            if (!DateTime.TryParse(dateRange.StartDate, out var startDate)
                & !DateTime.TryParse(dateRange.EndDate, out var endDate))
            {
                return BadRequest("Не верный формат даты!");
            }

            var operations =
                _unitOfWork.GetOperationsForPeriod(account, startDate, endDate, ref areThereOperations);

            if (areThereOperations)
            {
                return Ok(operations);
            }

            return NotFound("За данный период операций нет");
        }

        [HttpPut]
        public IActionResult UpdateOperation([FromBody] Operation transmittedOperation)
        {
            var foundOperation = _unitOfWork.GetOperationByID(transmittedOperation.OperationId);

            if (foundOperation == null)
            {
                return NotFound("Операция для изменения не найдена");
            }

            try
            {
                _unitOfWork.RegisterDirty(transmittedOperation);
                _unitOfWork.Commit();
                return Ok(foundOperation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _unitOfWork.Disposing(transmittedOperation);
                return BadRequest("Не получилось изменить операцию");
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOperation(int id)
        {
            var operation = _unitOfWork.GetOperationByID(id);
            if (operation == null)
            {
                return NotFound("Операция с таким ID не найдена");
            }

            try
            {
                _unitOfWork.RegisterDelete(operation);
                _unitOfWork.Commit();
                return Ok("Операция удалена");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _unitOfWork.Disposing(operation);
                return BadRequest("Не получилось удалить операцию");
            }
        }
    }
}