using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Core.InputOutputData;
using VostokZapadApp.Services.Interfaces;

namespace VostokZapadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        private readonly ISalesService _salesService;
        private readonly IValidateService _validateService;

        public SalesController(ISalesService salesService, IValidateService validateService)
        {
            _salesService = salesService;
            _validateService = validateService;
        }

        /// <summary>
        /// Получить все заказы.
        /// </summary>
        /// <returns></returns>
        [HttpGet("/all")]
        public async Task<ActionResult<List<Sales>>> GetAll()
        {
            return await _salesService.GetAllAsync();
        }

        /// <summary>
        /// Получить заказы по дате.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        [HttpGet("/bydate")]
        public async Task<ActionResult<List<Sales>>> GetByDate([FromHeader(Name = "Min-Date")]DateTime min, [FromHeader(Name = "Max-Date")]DateTime max)
        {
            if (min > max)
                return BadRequest();

            return await _salesService.GetByDateAsync(min, max);
        }

        /// <summary>
        /// Получить заказы по имени покупателя.
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns></returns>
        [HttpGet("/bycustomer")]
        public async Task<ActionResult<Sales>> GetByCustomer(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return BadRequest();

            return await _salesService.GetByCustomerAsync(customerName);
        }

        /// <summary>
        /// Получить заказ по номеру документа(не id базы).
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpGet("/bydocid")]
        public async Task<ActionResult<Sales>> GetById(int documentId)
        {
            if (documentId == 0)
                return BadRequest();

            return await _salesService.GetByDocIdAsync(documentId);
        }

        /// <summary>
        /// Добавить покупателя.
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns></returns>
        [HttpPost("/addcustomer")]
        public async Task<ActionResult> AddCustomer(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return BadRequest();

            return await _validateService.AddCustomer(customerName);
        }

        /// <summary>
        /// Добавить заказ.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="documentId"></param>
        /// <param name="sum"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        [HttpPost("/addorder")]
        public async Task<ActionResult> AddOrder(DateTime date, int documentId, decimal sum, string customerName) //По хорошему тут должена быть своя абстракция на входные данные.
        {
            if (date == DateTime.MinValue || documentId == 0 || sum <= 0 || string.IsNullOrWhiteSpace(customerName))
                return BadRequest();

            return await _validateService.AddOrder(date, documentId, sum, customerName);
        }
    }
}
