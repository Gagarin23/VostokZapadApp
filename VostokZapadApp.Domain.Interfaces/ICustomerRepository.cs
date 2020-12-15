using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core.DataBase;

namespace VostokZapadApp.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<ActionResult<Customer>> GetAsync(string customerName);
        Task<ActionResult<Customer>> GetAsync(int id);
        Task<ActionResult<int>> AddAsync(Customer customer);
        Task<ActionResult> UpdateAsync(Customer customer);
        Task<ActionResult> RemoveAsync(int id);
        Task<ActionResult> RemoveAsync(string customerName);
    }
}
