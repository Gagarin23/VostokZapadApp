using NUnit.Framework;
using VostokZapadApp.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Infrastructure.Data.Initialisation;

namespace VostokZapadApp.Infrastructure.Data.Tests
{
    [TestFixture]
    public class CustomerRepositoryTests
    {
        private const string ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=VostokZapadDb;Trusted_Connection=True;MultipleActiveResultSets=true";
        private CustomerRepository _rep;
        [SetUp]
        public async Task Init()
        {
            //var init = new DatabaseInitialiser("VostokZapadDb");
            //await init.CreateDatabase();
            //await init.CreateTables();

            var db = new SqlConnection(ConnectionString);
            _rep = new CustomerRepository(db);
        }

        [Test]
        public async Task GetAsyncTest()
        {
            var name = "TestName";

            var customer = await _rep.GetAsync(name);

            Assert.IsNotNull(customer);
        }

        [Test]
        public async Task AddAsyncTest()
        {
            var customer = new Customer {Name = "TestName"};

            var result = await _rep.AddOrUpdateAsync(customer);

            Assert.AreEqual(typeof(OkResult), result.GetType());
        }
    }
}