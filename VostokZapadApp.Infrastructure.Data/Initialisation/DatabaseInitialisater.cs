using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace VostokZapadApp.Infrastructure.Data.Initialisation
{
    public interface IDatabaseInitialiser
    {
        Task CreateDatabase();
        Task CreateTables();
        Task CreateCustomersProcedures();
        Task CreateOrdersProcedures();
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

            if(!_isDbCreated)
            {
                using (var db = new SqlConnection(InitConnection))
                {
                    await db.ExecuteAsync(query);
                    _isDbCreated = true;
                }
            }
        }

        public async Task CreateTables()
        {
            string queryShops = "CREATE TABLE Customers " +
                                "(Id INT IDENTITY PRIMARY KEY, " +
                                "Name NVARCHAR(50) UNIQUE)";

            string queryProducts = "CREATE TABLE Orders " +
                                   "(Id INT IDENTITY PRIMARY KEY, " +
                                   "DocDate date NOT NULL, " +
                                   "DocumentId INT UNIQUE NOT NULL, " +
                                   "OrderSum MONEY NOT NULL, " +
                                   "CustomerId INT NOT NULL REFERENCES Customers(Id) ON DELETE CASCADE)";

            if(!_isTablesCreated)
            {
                using (var db = new SqlConnection(_connectionString))
                {
                    await db.ExecuteAsync(queryShops);
                    await db.ExecuteAsync(queryProducts);
                    _isTablesCreated = true;
                }
            }
        }

        public async Task CreateCustomersProcedures()
        {
            var getByName = $"CREATE PROCEDURE {CustomerProcedures.GetCustomerByName}( " +
                            "@Name NVARCHAR(50)) AS " +
                            "SELECT TOP(1) Id, Name FROM Customers " +
                            "WHERE Name = @Name "; 

            var getById =  $"CREATE PROCEDURE {CustomerProcedures.GetCustomerById}( " +
                           "@Id INT) AS " +
                           "SELECT TOP(1) Id, Name FROM Customers " +
                           "WHERE Id = @Id";

            var add = $"CREATE PROCEDURE {CustomerProcedures.AddCustomer}( " +
                      "@Name NVARCHAR(50)) AS " +
                      "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Name = @Name) " +
                      "    RETURN 400 " +
                      "ELSE " +
                      "    BEGIN " +
                      "        INSERT INTO Customers (Name) VALUES (@Name) " +
                      "        RETURN 201 " +
                      "    END";
            
            //выдает ошибку если включаю return:
            //A MERGE statement must be terminated by a semi-colon (;).
            //куда лепить ; так и не понял.
            var updateOrInsert = $"CREATE PROCEDURE {CustomerProcedures.UpdateOrInsertCustomer}( " +
                                 "@Id INT, @Name NVARCHAR(50)) AS " +
                                 //"BEGIN " +
                                 "MERGE " +
                                 "INTO Customers WITH (HOLDLOCK) AS target " +
                                 "USING (SELECT @Id as id, @Name as name) as src (id, name) " +
                                 "ON (target.Id = src.id) " +
                                 "WHEN MATCHED " +
                                 "   THEN UPDATE " +
                                 "      SET target.Name = src.name " +
                                 //"      RETURN 200 " +
                                 "WHEN NOT MATCHED " +
                                 "   THEN INSERT(name) " +
                                 "      VALUES(src.name); ";
                                 //"      RETURN 400 \r\n" +
                                 //"; \r\n" +
                                 //"END;"

            var removeByName = $"CREATE PROCEDURE {CustomerProcedures.RemoveCustomerByName}( " +
                         "@Name NVARCHAR(50)) AS " +
                         "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Name = @Name) " +
                         "  BEGIN " +
                         "      DELETE FROM Customers WHERE Name = @Name " +
                         "      return 200 " +
                         "  END " +
                         "ELSE " +
                         "  return 404";

            var removeById = $"CREATE PROCEDURE {CustomerProcedures.RemoveCustomerById}( " +
                             "@Id INT) AS " +
                             "IF EXISTS (SELECT TOP(1) Id FROM Customers WHERE Id = @Id) " +
                             "  BEGIN " +
                             "      DELETE FROM Customers WHERE Id = @Id " +
                             "      return 200 " +
                             "  END " +
                             "ELSE " +
                             "  return 404";

            using (var db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(getByName);
                await db.ExecuteAsync(getById);
                await db.ExecuteAsync(add);
                await db.ExecuteAsync(updateOrInsert);
                await db.ExecuteAsync(removeByName);
                await db.ExecuteAsync(removeById);
            }
        }

        public async Task CreateOrdersProcedures()
        {
            var getAll = $"CREATE PROCEDURE {OrderProcedures.GetAllOrders} AS " +
                         "SELECT * FROM Orders";

            var getByDoc = $"CREATE PROCEDURE {OrderProcedures.GetOrderByDocumentId}( " +
                           "@DocId INT) AS " +
                           "SELECT TOP(1) * FROM ORDERS WHERE DocumentId = @DocId";


            var getByDate = $"CREATE PROCEDURE {OrderProcedures.GetOrdersByDate}( " +
                            "@MinDate DATE, @MaxDate DATE) AS " +
                            "SELECT * FROM Orders " +
                            "WHERE DocDate > @MinDate AND DocDate < @MaxDate";

            var getByCustomer = $"CREATE PROCEDURE {OrderProcedures.GetOrdersByCustomer}( " +
                                "@Name NVARCHAR(50)) AS " +
                                "SELECT * FROM Orders as O " +
                                "WHERE O.CustomerId = (SELECT TOP(1) Id " +
                                "FROM Customers as C " +
                                "WHERE C.Name = @Name)";

            var add = $"CREATE PROCEDURE {OrderProcedures.AddOrder}( " +
                      "@DocDate DATE, @DocId INT, @OrderSum MONEY, @CustomerId INT) AS " +
                      "INSERT INTO Orders (DocDate, DocumentId, OrderSum, CustomerId) " +
                      "VALUES (@DocDate, @DocId, @OrderSum, @CustomerId)";

            var updateOrInsert = ""; //todo: доделать процедуры для заказов. Очень хочу спать...

            var remove = "";

            using (var db = new SqlConnection(_connectionString))
            {
                await db.ExecuteAsync(getAll);
                await db.ExecuteAsync(getByDoc);
                await db.ExecuteAsync(getByDate);
                await db.ExecuteAsync(getByCustomer);
                await db.ExecuteAsync(add);
            }
        }
    }
}
