using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core.InputOutputData;

namespace VostokZapadApp.Services.Interfaces
{
    public interface ISalesService
    {
        Task<ActionResult<List<Sales>>> GetAllAsync();
        Task<ActionResult<List<Sales>>> GetByDateAsync(DateTime min, DateTime max);
        Task<ActionResult<Sales>> GetByCustomerAsync(string customerName);
        Task<ActionResult<Sales>> GetByDocIdAsync(int documentId);
    }
}
