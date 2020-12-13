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
    public class OrderRepository : IOrderRepository
    {
        public Task<ActionResult<Order>> GetByDocId(int documentId)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Add(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> UpdateOrInsert(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<ActionResult> Remove(int documentId)
        {
            throw new NotImplementedException();
        }
    }
}
