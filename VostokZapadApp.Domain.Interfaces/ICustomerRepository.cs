using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
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
