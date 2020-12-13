using NUnit.Framework;
using VostokZapadApp.Infrastructure.Data.Initialisation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VostokZapadApp.Infrastructure.Data.Initialisation.Tests
{
    [TestFixture()]
    public class DatabaseInitialiserTests
    {
        [Test()]
        public async Task CreateCustomersProceduresTest()
        {
            var init = new DatabaseInitialiser("VostokZapadDb");
            await init.CreateCustomersProcedures();
        }
    }
}