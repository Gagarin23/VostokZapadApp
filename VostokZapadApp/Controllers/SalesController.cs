using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core;

namespace VostokZapadApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesController : ControllerBase
    {
        [HttpGet]
        public Task<ActionResult<IEnumerable<Sales>>> GetAll()
        {
            
        }

        [HttpGet]
        public Task<ActionResult<Sales>> GetByDate(DateTime min, DateTime max)
        {
            
        }

        [HttpGet]
        public Task<ActionResult<Sales>> GetByCustomer(string customerName)
        {
            
        }

        [HttpGet]
        public Task<ActionResult<Sales>> GetById(int documentId)
        {
            
        }

        [HttpPost]
        public void Post(Sales sales)
        {

        }

        [HttpPatch]
        public void Patch(Sales sales)
        {
        }

        [HttpDelete]
        public void Delete(int id)
        {
        }
    }
}
