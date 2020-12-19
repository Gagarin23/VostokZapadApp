namespace VostokZapadApp.Infrastructure.Data.Initialisation
{
    public class OrderProcedures
    {
        public static readonly string GetAllOrders = "SELECT * FROM Orders";

        public static readonly string GetOrderByDocumentId = "SELECT TOP(1) * FROM ORDERS WHERE DocumentId = @DocId";

        public static readonly string GetOrdersByDate = "SELECT * FROM Orders \r\n" +
                                                        "WHERE DocDate > @MinDate AND DocDate < @MaxDate";

        public static readonly string GetOrdersByCustomer = "SELECT * FROM Orders as O \r\n" +
                                                            "WHERE O.CustomerId = (SELECT TOP(1) Id \r\n" +
                                                            "FROM Customers as C \r\n" +
                                                            "WHERE C.Name = @Name)";

        public static readonly string AddOrder =
            "IF NOT EXISTS (SELECT TOP(1) Id FROM Orders WHERE DocumentId = @DocId) \r\n" +
            "   BEGIN \r\n" +
            "       INSERT INTO Orders (DocDate, DocumentId, OrderSum, CustomerId) \r\n" +
            "       OUTPUT INSERTED.Id \r\n" +
            "       VALUES (@DocDate, @DocId, @OrderSum, @CustomerId) \r\n" +
            "   END \r\n";

        public static readonly string UpdateOrder =
            "IF EXISTS (SELECT TOP(1) Id FROM Orders WHERE DocumentId = @DocumentId) \r\n" +
            "  BEGIN \r\n" +
            "      UPDATE Orders \r\n" +
            "      SET DocDate = @DocDate, OrderSum = @OrderSum, CustomerId = @CustomerId \r\n" +
            "      OUTPUT INSERTED.Id \r\n" +
            "      WHERE DocumentId = @DocumentId \r\n" +
            "  END \r\n";

        public static readonly string RemoveOrder =
            "IF EXISTS (SELECT TOP(1) Id FROM Orders WHERE DocumentId = @DocumentId) \r\n" +
            "  BEGIN \r\n" +
            "      DELETE FROM Orders \r\n" +
            "      OUTPUT DELETED.Id \r\n" +
            "      WHERE DocumentId = @DocumentId \r\n" +
            "  END \r\n";
    }
}
