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
        void CreateCustomersProcedures();
        void CreateOrdersProcedures();
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

        public void CreateCustomersProcedures()
        {
            var procedures = new List<string>
            {
                $"CREATE PROCEDURE {CustomerProcedures.GetCustomerByName}( \r\n" +
                "@Name NVARCHAR(50)) AS \r\n" +
                "SELECT TOP(1) Id, Name FROM Customers \r\n" +
                "WHERE Name = @Name ",

                $"CREATE PROCEDURE {CustomerProcedures.GetCustomerById}( \r\n" +
                "@Id INT) AS \r\n" +
                "SELECT TOP(1) Id, Name FROM Customers \r\n" +
                "WHERE Id = @Id",

                $"CREATE PROCEDURE {CustomerProcedures.AddCustomer}( \r\n" +
                "@Name NVARCHAR(50)) AS \r\n" +
                "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Name = @Name) \r\n" +
                "    RETURN 400 \r\n" +
                "ELSE \r\n" +
                "    BEGIN \r\n" +
                "         INSERT INTO Customers (Name) \r\n" +
                "         OUTPUT INSERTED.Id \r\n" +
                "         VALUES (@Name) \r\n" +
                "         RETURN 201 \r\n" +
                "    END",

                $"CREATE PROCEDURE {CustomerProcedures.UpdateCustomer}( \r\n" +
                "@Id INT, @Name NVARCHAR(50)) AS \r\n" +
                "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Id = @Id) \r\n" +
                "  BEGIN \r\n" +
                "      UPDATE Customers SET Name = @Name \r\n" +
                "      WHERE Id = @Id \r\n" +
                "      RETURN 200 \r\n" +
                "  END \r\n" +
                "ELSE \r\n" +
                "  RETURN 404",

                $"CREATE PROCEDURE {CustomerProcedures.RemoveCustomerByName}( \r\n" +
                "@Name NVARCHAR(50)) AS \r\n" +
                "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Name = @Name) \r\n" +
                "  BEGIN \r\n" +
                "      DELETE FROM Customers WHERE Name = @Name \r\n" +
                "      return 200 \r\n" +
                "  END \r\n" +
                "ELSE \r\n" +
                "  return 404",

                $"CREATE PROCEDURE {CustomerProcedures.RemoveCustomerById}( \r\n" +
                "@Id INT) AS \r\n" +
                "IF EXISTS (SELECT TOP(1) Id FROM Customers WHERE Id = @Id) \r\n" +
                "  BEGIN \r\n" +
                "      DELETE FROM Customers WHERE Id = @Id \r\n" +
                "      return 200 \r\n" +
                "  END \r\n" +
                "ELSE \r\n" +
                "  return 404"
            };

            using (var db = new SqlConnection(_connectionString))
            {
                foreach (var procedure in procedures)
                {
                    try
                    {
                        db.Execute(procedure);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }

        public void CreateOrdersProcedures()
        {
            var procedures = new List<string>
            {
                $"CREATE PROCEDURE {OrderProcedures.GetAllOrders} AS \r\n" +
                "SELECT * FROM Orders",

                $"CREATE PROCEDURE {OrderProcedures.GetOrderByDocumentId}( \r\n" +
                "@DocId INT) AS \r\n" +
                "SELECT TOP(1) * FROM ORDERS WHERE DocumentId = @DocId",

                $"CREATE PROCEDURE {OrderProcedures.GetOrdersByDate}( \r\n" +
                "@MinDate DATE, @MaxDate DATE) AS \r\n" +
                "SELECT * FROM Orders \r\n" +
                "WHERE DocDate > @MinDate AND DocDate < @MaxDate",

                $"CREATE PROCEDURE {OrderProcedures.GetOrdersByCustomer}( \r\n" +
                "@Name NVARCHAR(50)) AS \r\n" +
                "SELECT * FROM Orders as O \r\n" +
                "WHERE O.CustomerId = (SELECT TOP(1) Id \r\n" +
                "FROM Customers as C \r\n" +
                "WHERE C.Name = @Name)",

                $"CREATE PROCEDURE {OrderProcedures.AddOrder}( \r\n" +
                "@DocDate DATE, @DocId INT, @OrderSum MONEY, @CustomerId INT) AS \r\n" +
                "IF EXISTS (SELECT TOP(1) Id FROM Orders WHERE DocumentId = @DocId) \r\n" +
                "   BEGIN \r\n" +
                "       INSERT INTO Orders (DocDate, DocumentId, OrderSum, CustomerId) \r\n" +
                "       OUTPUT INSERTED.Id \r\n" +
                "       VALUES (@DocDate, @DocId, @OrderSum, @CustomerId) \r\n" +
                "       RETURN 201 \r\n" +
                "   END \r\n" +
                "ELSE \r\n" +
                "   RETURN 400",

                $"CREATE PROCEDURE {OrderProcedures.UpdateOrder}( \r\n" +
                "@DocDate DATE, @DocumentId INT, @OrderSum MONEY, @CustomerId INT) AS \r\n" +
                "IF EXISTS (SELECT TOP(1) Id FROM Orders WHERE DocumentId = @DocumentId) \r\n" +
                "  BEGIN \r\n" +
                "      UPDATE Orders \r\n" +
                "      SET DocDate = @DocDate, OrderSum = @OrderSum, CustomerId = @CustomerId \r\n" +
                "      WHERE DocumentId = @DocumentId \r\n" +
                "      RETURN 200 \r\n" +
                "  END \r\n" +
                "ELSE \r\n" +
                "  RETURN 404",

                $"CREATE PROCEDURE {OrderProcedures.RemoveOrder}( \r\n" +
                "@DocumentId INT) AS \r\n" +
                "IF EXISTS (SELECT TOP(1) Id FROM Orders WHERE DocumentId = @DocumentId) \r\n" +
                "  BEGIN \r\n" +
                "      DELETE FROM Orders WHERE DocumentId = @DocumentId \r\n" +
                "      return 200 \r\n" +
                "  END \r\n" +
                "ELSE \r\n" +
                "  return 404"
            };

            using (var db = new SqlConnection(_connectionString))
            {
                foreach (var procedure in procedures)
                {
                    try
                    {
                        db.Execute(procedure);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
        }
    }
}
