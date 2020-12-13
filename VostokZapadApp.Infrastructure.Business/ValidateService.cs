using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Services.Interfaces;

namespace VostokZapadApp.Infrastructure.Business
{
    public class ValidateService : IValidaterService
    {
        public Task<ActionResult<Sales>> Validate(Sales sales)
        {
            throw new System.NotImplementedException();
        }
    }
}
