using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Interfaces;

namespace VostokZapadApp.Infrastructure.Data
{
    /// <summary>
    ///     По хорошему, я считаю нужно подтягивать хранимые процедуры, а не хардкодить строками.
    /// </summary>
    public class OrderRepository : IOrderRepository
    {
        private readonly IDbConnection _db;

        public OrderRepository(IDbConnection db)
        {
            _db = db;
        }

        public async Task<ActionResult<List<Order>>> GetAllAsync()
        {
            var sql = "SELECT * FROM ORDERS";

            var result = await _db.QueryAsync<Order>(sql) as List<Order> ?? new List<Order>();
            if(result.Count < 1)
                return new NotFoundResult();

            return result;
        }

        public async Task<ActionResult<Order>> GetByDocIdAsync(int documentId)
        {
            var sql = "SELECT TOP(1) * FROM ORDERS as O " +
                      "WHERE O.DocumentId = @documentId";

            var result = await _db.QueryFirstOrDefaultAsync<Order>(sql, new{documentId});
            if(result == null)
                return new NotFoundResult();

            return result;
        }

        public async Task<ActionResult<List<Order>>> GetByDateAsync(DateTime minDate, DateTime maxDate)
        {
            var sql = "SELECT * FROM Orders as O " +
                      "WHERE O.DocDate > @minDate AND O.DocDate < @maxDate";

            var result = await _db.QueryAsync<Order>(sql, new {minDate, maxDate}) as List<Order> ?? new List<Order>();
            if (result.Count < 1)
                return new NotFoundResult();

            return result;
        }

        public async Task<ActionResult<List<Order>>> GetByCustomerAsync(Customer customer)
        {
            var sql = "SELECT * FROM ORDERS as O " +
                      "WHERE O.CustomerId = (SELECT TOP(1) Id " +
                      "FROM Customers as C " +
                      "WHERE C.Name = @name)";

            var parameters = new DynamicParameters();
            parameters.Add("@name", customer.Name);

            var result = await _db.QueryAsync<Order>(sql, parameters) as List<Order> ?? new List<Order>();
            if (result.Count < 1)
                return new NotFoundResult();

            return result;
        }

        public async Task<ActionResult> AddAsync(Order order)
        {
            var sql = "INSERT INTO Orders (DocDate, DocumentId, OrderSum, CustomerId) " +
                      "VALUES (@docDate, @docId, @orderSum, @customerId)";

            var parameters = new DynamicParameters();
            parameters.Add("@docDate", order.DateTime, DbType.Date, ParameterDirection.Input);
            parameters.Add("@docId", order.DocumentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@orderSum", order.OrderSum, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@customerId", order.CustomerId, DbType.Int32, ParameterDirection.Input);

            var result = await _db.ExecuteAsync(sql, parameters);
            if(result > 0)
                return new StatusCodeResult(201);

            return new StatusCodeResult(500);
        }

        public async Task<ActionResult> UpdateOrInsertAsync(Order order)
        {
            var sql = "MERGE " +
                      "INTO Orders WITH (HOLDLOCK) AS target " +
                      "USING (SELECT " +
                      "@docDate as docDate," +
                      "@docId as docId," +
                      "@orderSum as orderSum," +
                      "@customerId as customerId) as src (docDate, docId, orderSum, customerId) " +
                      "ON (target.DocumentId = src.docId) " +
                      "WHEN MATCHED " +
                      "   THEN UPDATE " +
                      "       SET target.DocDate = src.docDate, " +
                      "         target.DocId = src.docId, " +
                      "         target.OrderSum = src.orderSum," +
                      "         target.CustomerId = src.customerId" +
                      "WHEN NOT MATCHED " +
                      "   THEN INSERT(docDate, docId, orderSum, customerId) " +
                      "       VALUES(src.docDate, src.docId, src.orderSum, src.customerId);";

            var parameters = new DynamicParameters();
            parameters.Add("@docDate", order.DateTime, DbType.Date, ParameterDirection.Input);
            parameters.Add("@docId", order.DocumentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@orderSum", order.OrderSum, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@customerId", order.CustomerId, DbType.Int32, ParameterDirection.Input);

            var result = await _db.ExecuteAsync(sql, parameters);
            if(result > 1)
                return new OkResult();

            return new StatusCodeResult(500);
        }

        public async Task<ActionResult> RemoveAsync(int documentId)
        {
            throw new NotImplementedException();
        }
    }
}
