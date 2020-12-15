using Dapper;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System.Collections.Generic;
using VostokZapadApp.Domain.Core.DataBase;

namespace VostokZapadApp.Infrastructure.DataTests
{
    class SqlOutputTest
    {
        [Test]
        public void GetValue()
        {
            var sql = "INSERT INTO Customers (Name) " +
                      "OUTPUT inserted.Id " +
                      "VALUES ('test1545')";
            int id;

            using (var db =
                new SqlConnection(
                    "Server=(localdb)\\mssqllocaldb;Database=VostokZapadDb;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                id = db.QueryFirst<int>(sql);
            }

            Assert.AreNotEqual(0, id);
        }

        [Test]
        public void GerOrder()
        {
            var sql = "Select * From Orders";
            IEnumerable<Order> orders;

            using (var db =
                new SqlConnection(
                    "Server=(localdb)\\mssqllocaldb;Database=VostokZapadDb;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                orders = db.Query<Order>(sql);
            }


        }
    }
}
