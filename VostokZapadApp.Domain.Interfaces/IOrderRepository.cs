using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core;

namespace VostokZapadApp.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<ActionResult<List<Order>>> GetAll();
        Task<ActionResult<Order>> GetByDocId(int documentId);
        Task<ActionResult<List<Order>>> GetByDate(DateTime min, DateTime max);
        Task<ActionResult> Add(Order order);
        Task<ActionResult> UpdateOrInsert(Order order);
        Task<ActionResult> Remove(int documentId);
    }
}
