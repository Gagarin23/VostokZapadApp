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
        Task<ActionResult> AddOrUpdateAsync(Customer customer);
        Task<ActionResult> UpdateOrInsertAsync(Customer customer);
        Task<ActionResult> RemoveAsync(string customerName);
    }
}
