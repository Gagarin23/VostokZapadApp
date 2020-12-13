using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;

namespace VostokZapadApp.Domain.Interfaces
{
    public interface ICustomerRepository
    {
        Task<ActionResult<Customer>> GetByDocId(int documentId);
        Task<ActionResult> Add(Customer customer);
        Task<ActionResult> UpdateOrInsert(Customer customer);
        Task<ActionResult> Remove(int customerName);
    }
}
