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

        public async Task<ActionResult> AddOrderAsync(DateTime date, int documentId, decimal sum, string customerName)
        {
            #region КостыльЗаКоторыйМнеСтыдно //todo: если будет время, делегировать и оптимизировать в запросе sql

            var customer = (await _customerRepository.GetAsync(customerName)).Value;
            if (customer == null)
            {
                await _customerRepository.AddAsync(new Customer{Name = customerName});
                customer = (await _customerRepository.GetAsync(customerName)).Value;
            }

            #endregion

            //...еще какая-то валидация.

            var order = new Order
            {
                DateTime = date,
                DocumentId = documentId,
                OrderSum = sum,
                CustomerId = customer.Id
            };

            return await _orderRepository.AddAsync(order);
        }

    }
}
