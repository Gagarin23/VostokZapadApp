using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Interfaces;

namespace VostokZapadApp.Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        public async Task<ActionResult<Customer>> GetAsync(string customerName)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> AddAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> UpdateOrInsertAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> RemoveAsync(string customerName)
        {
            throw new NotImplementedException();
        }
    }
}
