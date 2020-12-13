using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
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
