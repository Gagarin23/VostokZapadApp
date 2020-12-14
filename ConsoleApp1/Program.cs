using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace ConsoleApp1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            DateTime minDate = DateTime.MinValue;
            DateTime maxDate = DateTime.MaxValue;


            var parameters = new DynamicParameters();
            parameters.Add("@MinDate", minDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);
            parameters.Add("@MaxDate", maxDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);
            //parameters.Add("@dt", dbType: DbType.Date, direction: ParameterDirection.Output);

            using (var db =
                new SqlConnection(
                    "Server=(localdb)\\mssqllocaldb;Database=VostokZapadDb;Trusted_Connection=True;MultipleActiveResultSets=true")
            )
            {
                var reader = await db.ExecuteReaderAsync(
                    "GetOrdersByDate", parameters, commandType: CommandType.StoredProcedure);

                while (reader.HasRows)
                {
                    Console.WriteLine("\t{0}\t{1}", reader.GetName(0),
                        reader.GetName(1));

                    while (reader.Read())
                    {
                        Console.WriteLine("\t{0}\t{1}", reader.GetInt32(0),
                            reader.GetDateTime(1));
                    }

                    reader.NextResult();
                }

            }

            Console.ReadLine();
        }
    }
}
