using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Interfaces;

namespace VostokZapadApp.Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        public Task<ActionResult<Customer>> GetByDocId(int documentId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Add(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateOrInsert(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Remove(int customerName)
        {
            throw new NotImplementedException();
        }
    }
}
