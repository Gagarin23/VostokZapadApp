using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
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
                      "VALUES ('test14')";
            var p = new DynamicParameters();
            p.Add("@@Identity", dbType:DbType.Int32, direction: ParameterDirection.Output);
            
            using (var db =
                new SqlConnection(
                    "Server=(localdb)\\mssqllocaldb;Database=VostokZapadDb;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var a = db.Query<int>(sql);
                db.Execute(sql,p);
            }
            var result = p.Get<int>("@@Identity");
            
            Assert.AreNotEqual(0, result);
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
