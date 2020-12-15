using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VostokZapadApp.Services.Interfaces
{
    public interface ICustomersValidateService
    {
        Task<ActionResult<int>> AddAsync(string customerName);
        Task<ActionResult> UpdateAsync(int id, string customerName);
    }
}
