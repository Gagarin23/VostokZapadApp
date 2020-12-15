using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace VostokZapadApp.Services.Interfaces
{
    public interface IOrdersValidateService
    {
        Task<ActionResult<int>> AddOrderAsync(DateTime date, int documentId, decimal sum, string customerName);
    }
}
