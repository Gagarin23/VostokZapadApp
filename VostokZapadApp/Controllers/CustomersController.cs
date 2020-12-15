using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Interfaces;
using VostokZapadApp.Services.Interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VostokZapadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomersValidateService _validateService;
        private readonly ICustomerRepository _customerRepository;

        public CustomersController(ICustomersValidateService validateService, ICustomerRepository customerRepository)
        {
            _validateService = validateService;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Получить клиента по имени.
        /// </summary>
        /// <param customerName="customerName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<Customer>> Get(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return BadRequest();

            return await _customerRepository.GetAsync(customerName);
        }

        /// <summary>
        /// Получить клиента по id.
        /// </summary>
        /// <param id="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(int id)
        {
            if (id < 1)
                return BadRequest();

            return await _customerRepository.GetAsync(id);
        }

        /// <summary>
        /// Добавить клиента.
        /// </summary>
        /// <param customerName="customerName"></param>
        /// <returns>Id клиента.</returns>
        [HttpPost("/add")]
        public async Task<ActionResult<int>> AddCustomer(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return BadRequest();

            return await _validateService.AddAsync(customerName);
        }

        /// <summary>
        /// Изменить имя клиента.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="customerName"></param>
        /// <returns></returns>
        [HttpPatch("/update/{id}")]
        public async Task<ActionResult> Update(int id, string customerName)
        {
            if (id < 1 || string.IsNullOrWhiteSpace(customerName))
                return BadRequest();

            return await _validateService.UpdateAsync(id, customerName);
        }

        /// <summary>
        /// Удалить клиента по id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/del/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id < 1)
                return BadRequest();

            return await _customerRepository.RemoveAsync(id);
        }

        /// <summary>
        /// Удалить клиента по имени.
        /// </summary>
        /// <param name="customerName"></param>
        /// <returns></returns>
        [HttpDelete("/del/name={customerName}")]
        public async Task<ActionResult> Delete(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return BadRequest();

            return await _customerRepository.RemoveAsync(customerName);
        }
    }
}
