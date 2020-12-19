using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VostokZapadApp.Infrastructure.Data.Initialisation
{
    public interface IDatabaseInitialiser
    {
        Task CreateDatabase();
        void CreateTables();
    }

    public class DatabaseInitialiser : IDatabaseInitialiser
    {
        private readonly string _databaseName;

        private const string InitConnection =
            "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;MultipleActiveResultSets=true";

        private string _connectionString;

        private bool _isDbCreated;
        private bool _isTablesCreated;

        public DatabaseInitialiser(string databaseName)
        {
            _databaseName = databaseName;
            _connectionString = $"Server=(localdb)\\mssqllocaldb;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true";
        }

        public async Task CreateDatabase()
        {
            string query = $"CREATE DATABASE {_databaseName}";

            if (!_isDbCreated)
            {
                using (var db = new SqlConnection(InitConnection))
                {
                    await db.ExecuteAsync(query);
                    _isDbCreated = true;
                }
            }
        }

        public void CreateTables()
        {
            var procedures = new List<string>
            {
                "CREATE TABLE Customers " +
                "(Id INT IDENTITY PRIMARY KEY, " +
                "Name NVARCHAR(50) UNIQUE)",

                "CREATE TABLE Orders " +
                "(Id INT IDENTITY PRIMARY KEY, " +
                "DocDate date NOT NULL, " +
                "DocumentId INT UNIQUE NOT NULL, " +
                "OrderSum MONEY NOT NULL, " +
                "CustomerId INT NOT NULL REFERENCES Customers(Id) ON DELETE CASCADE)"
            };

            if (!_isTablesCreated)
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    foreach (var procedure in procedures)
                    {
                        try //ещё один костыль дабы не выпадать в исключение.
                        {
                            db.Execute(procedure);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message); //+ логгирование.
                        }
                    }

                    _isTablesCreated = true;
                }
            }
        }
    }
}
