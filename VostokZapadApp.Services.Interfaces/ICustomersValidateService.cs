using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VostokZapadApp.Services.Interfaces
{
    public interface ICustomersValidateService
    {
        Task<ActionResult> AddAsync(string customerName);
        Task<ActionResult> UpdateAsync(int id, string customerName);
    }
}
