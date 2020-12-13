using NUnit.Framework;
using VostokZapadApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace VostokZapadApp.Infrastructure.Data.Tests
{
    [TestFixture()]
    public class CustomerRepositoryTests
    {
        [Test()]
        public async Task RemoveAsyncTest()
        {
            var rep = new CustomerRepository(new SqlConnection(
                "Server=(localdb)\\mssqllocaldb;Database=VostokZapadDb;Trusted_Connection=True;MultipleActiveResultSets=true"));

            await rep.RemoveAsync("TestName2");
        }
    }
}