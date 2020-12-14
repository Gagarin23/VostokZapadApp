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
        private DatabaseInitialiser _init;
        [SetUp]
        public void Init()
        {
            _init = new DatabaseInitialiser("VostokZapadDb");
        }
        [Test()]
        public async Task CreateCustomersProceduresTest()
        {
            await _init.CreateCustomersProcedures();
        }

        [Test()]
        public async Task CreateOrdersProceduresTest()
        {
            await _init.CreateOrdersProcedures();
        }
    }
}