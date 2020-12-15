using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;

namespace VostokZapadApp.Domain.Interfaces
{
    public interface IOrderRepository
    {
        Task<ActionResult<List<Order>>> GetAllAsync();
        Task<ActionResult<Order>> GetByDocIdAsync(int documentId);
        Task<ActionResult<List<Order>>> GetByDateAsync(DateTime min, DateTime max);
        Task<ActionResult<List<Order>>> GetByCustomerAsync(Customer customer);
        Task<ActionResult<int>> AddAsync(Order order);
        Task<ActionResult> UpdateOrInsertAsync(Order order);
        Task<ActionResult> RemoveAsync(int documentId);
    }
}
