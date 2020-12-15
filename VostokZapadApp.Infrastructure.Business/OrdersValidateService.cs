using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Interfaces;
using VostokZapadApp.Services.Interfaces;

namespace VostokZapadApp.Infrastructure.Business
{
    public class OrdersValidateService : IOrdersValidateService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomersValidateService _customersValidate;
        private readonly ICustomerRepository _customerRepository;

        public OrdersValidateService(IOrderRepository orderRepository, ICustomersValidateService customersValidate, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customersValidate = customersValidate;
            _customerRepository = customerRepository;
        }

        public async Task<ActionResult<int>> AddOrderAsync(DateTime date, int documentId, decimal sum, string customerName)
        {
            var id = (await _customerRepository.GetAsync(customerName)).Value.Id;
            if (id == 0)
                return new ObjectResult("Клиент не найден.") {StatusCode = 404};

            //...еще какая-то валидация.

            var order = new Order
            {
                DocDate = date,
                DocumentId = documentId,
                OrderSum = sum,
                CustomerId = id
            };

            return await _orderRepository.AddAsync(order);
        }

    }
}
