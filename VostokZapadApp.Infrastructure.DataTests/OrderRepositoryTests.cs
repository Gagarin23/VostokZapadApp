using Dapper;
using Microsoft.Data.SqlClient;
using NUnit.Framework;
using System;
using System.Data;
using System.Threading.Tasks;
using VostokZapadApp.Infrastructure.Data.Initialisation;

namespace VostokZapadApp.Infrastructure.Data.Tests
{
    [TestFixture()]
    public class OrderRepositoryTests
    {
        [Test()]
        public async Task GetByDateAsyncTest()
        {
            DateTime minDate = DateTime.MinValue;
            DateTime maxDate = DateTime.MaxValue;


            var parameters = new DynamicParameters();
            parameters.Add("@MinDate", minDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);
            parameters.Add("@MaxDate", maxDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);
            parameters.Add("@dt", dbType: DbType.Date, direction: ParameterDirection.Output);

            var getByDate = $"CREATE PROCEDURE {OrderProcedures.GetOrdersByDate}( " +
                            "@MinDate DATE, @MaxDate DATE) AS " +
                            "SELECT * FROM Orders " +
                            "WHERE DocDate > @MinDate AND DocDate < @MaxDate";

            using (var db = new SqlConnection("Server=(localdb)\\mssqllocaldb;Database=VostokZapadDb;Trusted_Connection=True;MultipleActiveResultSets=true"))
            {
                var reader = await db.ExecuteReaderAsync(
                    "GetOrdersByDate", parameters);

                while (reader.HasRows)
                {
                    Console.WriteLine("\t{0}\t{1}", reader.GetName(0),
                        reader.GetName(1));

                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}", reader.GetInt32(0),
                            reader.GetString(1));
                    }
                    reader.NextResult();
                }

                var dt = parameters.Get<DateTime>("@dt");
            }

        }
    }
}