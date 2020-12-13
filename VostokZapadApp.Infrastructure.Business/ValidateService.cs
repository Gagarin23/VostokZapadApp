using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Interfaces;
using VostokZapadApp.Services.Interfaces;

namespace VostokZapadApp.Infrastructure.Business
{
    public class ValidateService : IValidateService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public ValidateService(IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task<ActionResult> AddOrder(DateTime date, int documentId, decimal sum, string customerName)
        {
            var customer = (await _customerRepository.GetAsync(customerName)).Value;
            if (customer == null)
            {
                await AddCustomer(customerName);
                customer = (await _customerRepository.GetAsync(customerName)).Value;
                if(customer == null)
                    return new StatusCodeResult(500);
            }
            //...еще какая-то валидация.

            var order = new Order
            {
                DateTime = date,
                DocumentId = documentId,
                OrderSum = sum,
                Customer = customer,
                CustomerId = customer.Id
            };

            var result = await _orderRepository.AddAsync(order);
            if(result.Value == null)
                return new StatusCodeResult(500);

            return new OkResult();
        }

        public async Task<ActionResult> AddCustomer(string customerName)
        {
            var customer = new Customer{Name = customerName};
            return await _customerRepository.AddAsync(customer);
        }
    }
}
