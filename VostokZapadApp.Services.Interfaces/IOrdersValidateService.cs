using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;

namespace VostokZapadApp.Services.Interfaces
{
    public interface IOrdersValidateService
    {
        Task<ActionResult<int>> AddOrderAsync(DateTime date, int documentId, decimal sum, string customerName);
    }
}
