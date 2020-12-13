using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Core.InputOutputData;
using VostokZapadApp.Domain.Interfaces;
using VostokZapadApp.Services.Interfaces;

namespace VostokZapadApp.Infrastructure.Business
{
    public class SalesService : ISalesService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;

        public SalesService(IOrderRepository orderRepository, ICustomerRepository customerRepository)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
        }

        public async Task<ActionResult<List<Sales>>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            return await GetSalesAsync(orders.Value);
        }

        public async Task<ActionResult<List<Sales>>> GetByDateAsync(DateTime min, DateTime max)
        {
            var orders = await _orderRepository.GetByDateAsync(min, max);
            return await GetSalesAsync(orders.Value);
        }

        private async Task<List<Sales>> GetSalesAsync(List<Order> orders)
        {
            var customersIds = orders.Select(x => x.CustomerId).ToList();

            var sales = new List<Sales>();
            foreach (var id in customersIds)
            {
                sales.Add(new Sales
                {
                    Customer = (await _customerRepository.GetAsync(id)).Value,
                    Orders = orders.Where(x => x.CustomerId == id).ToList()
                });
            }

            return sales;
        }

        public async Task<ActionResult<Sales>> GetByCustomerAsync(string customerName)
        {
            var customer = new Customer{Name = customerName};
            var orders = await _orderRepository.GetByCustomerAsync(customer);
            customer.Id = orders.Value.First().CustomerId;
            return new Sales
            {
                Customer = customer,
                Orders = orders.Value
            };
        }

        public async Task<ActionResult<Sales>> GetByDocIdAsync(int documentId)
        {
            var order = await _orderRepository.GetByDocIdAsync(documentId);
            return new Sales
            {
                Customer = (await _customerRepository.GetAsync(order.Value.CustomerId)).Value,
                Orders = new List<Order> {order.Value}
            };
        }
    }
}
