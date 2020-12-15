using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System.Threading.Tasks;

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