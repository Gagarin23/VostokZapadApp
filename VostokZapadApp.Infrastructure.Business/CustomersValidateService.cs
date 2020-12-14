using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Interfaces;
using VostokZapadApp.Services.Interfaces;

namespace VostokZapadApp.Infrastructure.Business
{
    public class CustomersValidateService : ICustomersValidateService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomersValidateService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<ActionResult> AddAsync(string customerName)
        {
            if(string.IsNullOrWhiteSpace(customerName))
                return new BadRequestResult();

            var customer = new Customer{Name = customerName};
            return await _customerRepository.AddAsync(customer);
        }

        public async Task<ActionResult> UpdateAsync(int id, string customerName)
        {
            if(id < 1 || string.IsNullOrWhiteSpace(customerName))
                return new BadRequestResult();

            var customer = new Customer
            {
                Id = id,
                Name = customerName
            };
            return await _customerRepository.UpdateAsync(customer);
        }
    }
}
