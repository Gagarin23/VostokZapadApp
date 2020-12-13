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
    public class OrderRepository : IOrderRepository
    {
        public async Task<ActionResult<List<Order>>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<Order>> GetByDocIdAsync(int documentId)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<List<Order>>> GetByDateAsync(DateTime min, DateTime max)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult<List<Order>>> GetByCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult<Order>> AddAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> UpdateOrInsertAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> RemoveAsync(int documentId)
        {
            throw new NotImplementedException();
        }
    }
}
