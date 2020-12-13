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

        public SalesService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<ActionResult<List<Sales>>> GetAllAsync()
        {
            var orders = await _orderRepository.GetAllAsync();
            var customers = orders.Value.Select(x => new Customer {Id = x.CustomerId, Name = x.Customer.Name}).ToList();

            var allSales = new List<Sales>();
            foreach (var customer in customers)
            {
                allSales.Add(new Sales
                {
                    Customer = customer,
                    Orders = orders.Value.Where(x => x.CustomerId == customer.Id).ToList()
                });
            }

            return allSales;
        }

        public async Task<ActionResult<List<Sales>>> GetByDateAsync(DateTime min, DateTime max)
        {
            var orders = await _orderRepository.GetByDateAsync(min, max);
            var customers = orders.Value.Select(x => new Customer {Id = x.CustomerId, Name = x.Customer.Name}).ToList();

            var salesByDate = new List<Sales>();
            foreach (var customer in customers)
            {
                salesByDate.Add(new Sales
                {
                    Customer = customer,
                    Orders = orders.Value.Where(x => x.CustomerId == customer.Id).ToList()
                });
            }

            return salesByDate;

        }

        public async Task<ActionResult<Sales>> GetByCustomerAsync(string customerName)
        {
            var customer = new Customer{Name = customerName};
            var orders = await _orderRepository.GetByCustomerAsync(customer);
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
                Customer = order.Value.Customer,
                Orders = new List<Order> {order.Value}
            };
        }
    }
}
