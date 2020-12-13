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
    public interface IValidateService
    {
        Task<ActionResult> AddOrder(DateTime date, int documentId, decimal sum, string customerName);
        Task<ActionResult> AddCustomer(string customerName);
    }
}
