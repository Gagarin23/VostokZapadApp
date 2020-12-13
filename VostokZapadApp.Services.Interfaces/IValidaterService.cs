using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core;

namespace VostokZapadApp.Services.Interfaces
{
    public interface IValidaterService
    {
        Task<ActionResult<Sales>> Validate(Sales sales);
    }
}
