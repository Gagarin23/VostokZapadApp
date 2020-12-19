namespace VostokZapadApp.Infrastructure.Data.Initialisation
{
    /// <summary>
    /// Класс нужен для связи между хранимыми процедурами базы и приложением.
    /// </summary>
    class CustomerProcedures
    {
        public static readonly string GetCustomerByName = "SELECT TOP(1) Id, Name FROM Customers \r\n" +
                                                          "WHERE Name = @Name";

        public static readonly string GetCustomerById = "SELECT TOP(1) Id, Name FROM Customers \r\n" +
                                                        "WHERE Id = @Id";

        public static readonly string AddCustomer =
            "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Name = @Name) \r\n" +
            "    RETURN 400 \r\n" +
            "ELSE \r\n" +
            "    BEGIN \r\n" +
            "         INSERT INTO Customers (Name) \r\n" +
            "         OUTPUT INSERTED.Id \r\n" +
            "         VALUES (@Name) \r\n" +
            "         RETURN 201 \r\n" +
            "    END";

        public static readonly string UpdateCustomer =
            "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Id = @Id) \r\n" +
            "  BEGIN \r\n" +
            "      UPDATE Customers SET Name = @Name \r\n" +
            "      WHERE Id = @Id \r\n" +
            "      RETURN 200 \r\n" +
            "  END \r\n" +
            "ELSE \r\n" +
            "  RETURN 404";

        public static readonly string RemoveCustomerByName =
            "IF EXISTS (SELECT TOP(1) Name FROM Customers WHERE Name = @Name) \r\n" +
            "  BEGIN \r\n" +
            "      DELETE FROM Customers WHERE Name = @Name \r\n" +
            "      return 200 \r\n" +
            "  END \r\n" +
            "ELSE \r\n" +
            "  return 404";

        public static readonly string RemoveCustomerById =
            "IF EXISTS (SELECT TOP(1) Id FROM Customers WHERE Id = @Id) \r\n" +
            "  BEGIN \r\n" +
            "      DELETE FROM Customers WHERE Id = @Id \r\n" +
            "      return 200 \r\n" +
            "  END \r\n" +
            "ELSE \r\n" +
            "  return 404";
    }
}
