using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace VostokZapadApp.Infrastructure.Data.Initialisation
{
    public interface IDatabaseInitialiser
    {
        Task CreateDatabase();
        Task CreateTables();
    }

    public class DatabaseInitialiser : IDatabaseInitialiser
    {
        private readonly string _databaseName;

        private const string ConnectionString =
            "Server=(localdb)\\mssqllocaldb;Trusted_Connection=True;MultipleActiveResultSets=true";

        private bool _isDbCreated;
        private bool _isTablesCreated;

        public DatabaseInitialiser(string databaseName)
        {
            _databaseName = databaseName;
        }

        public async Task CreateDatabase()
        {
            string query = $"CREATE DATABASE {_databaseName}";

            if(!_isDbCreated)
            {
                using (var db = new SqlConnection(ConnectionString))
                {
                    await db.ExecuteAsync(query);
                    _isDbCreated = true;
                }
            }
        }

        public async Task CreateTables()
        {
            string queryShops = $"USE {_databaseName} " +
                                "CREATE TABLE Customers " +
                                "(Id INT IDENTITY PRIMARY KEY, " +
                                "Name NVARCHAR(50))";

            string queryProducts = $"USE {_databaseName} " +
                                   "CREATE TABLE Orders " +
                                   "(Id INT IDENTITY PRIMARY KEY, " +
                                   "DocDate date NOT NULL, " +
                                   "DocumentId INT UNIQUE NOT NULL, " +
                                   "OrderSum MONEY NOT NULL, " +
                                   "CustomerId INT NOT NULL REFERENCES Customers(Id) ON DELETE CASCADE)";

            if(!_isTablesCreated)
            {
                using (var db = new SqlConnection(ConnectionString))
                {
                    await db.ExecuteAsync(queryShops);
                    await db.ExecuteAsync(queryProducts);
                    _isTablesCreated = true;
                }
            }
        }
    }
}
