﻿using System.Data;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;

namespace VostokZapadApp.Infrastructure.Data.Initialisation
{
    public interface IDatabaseInitialiser
    {
        void CreateDatabase();
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

        public void CreateDatabase()
        {
            string query = $"CREATE DATABASE {_databaseName}";

            if(!_isDbCreated)
            {
                using (var db = new SqlConnection(InitConnection))
                {
                    db.Execute(query);
                    _isDbCreated = true;
                }
            }
        }

        public void CreateTables()
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
                    db.Execute(queryShops);
                    db.Execute(queryProducts);
                    _isTablesCreated = true;
                }
            }
        }

        public void CreateCustomersProcedures()
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
            
            
            var update = $"CREATE PROCEDURE {CustomerProcedures.UpdateCustomer}( " +
                                 "@Id INT, @Name NVARCHAR(50)) AS " +
                                 "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Id = @Id) " +
                                 "  BEGIN " +
                                 "      UPDATE Customers SET Name = @Name " +
                                 "      WHERE Id = @Id " +
                                 "      RETURN 200 " +
                                 "  END " +
                                 "ELSE " +
                                 "  RETURN 404";

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
                db.Execute(getByName);
                db.Execute(getById);
                db.Execute(add);
                db.Execute(update);
                db.Execute(removeByName);
                db.Execute(removeById);
            }
        }

        public void CreateOrdersProcedures()
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

            var updateOrInsert = $"CREATE PROCEDURE {OrderProcedures.UpdateOrder}( "; //todo: доделать процедуры для заказов. Очень хочу спать...

            var remove = ""; 

            using (var db = new SqlConnection(_connectionString))
            {
                db.Execute(getAll);
                db.Execute(getByDoc);
                db.Execute(getByDate);
                db.Execute(getByCustomer);
                db.Execute(add);
            }
        }
    }
}
