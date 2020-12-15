using NUnit.Framework;

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
        public void CreateCustomersProceduresTest()
        {
            _init.CreateCustomersProcedures();
        }

        [Test()]
        public void CreateOrdersProceduresTest()
        {
            _init.CreateOrdersProcedures();
        }
    }
}