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

            return await _db.QueryAsync<Order>(sql) as List<Order> ?? throw new Exception("Empty result from db.");
        }

        public async Task<ActionResult<Order>> GetByDocIdAsync(int documentId)
        {
            var sql = "SELECT TOP(1) * FROM ORDERS as O" +
                      "WHERE O.DocumentId = @documentId";

            return await _db.QueryFirstOrDefaultAsync<Order>(sql, new{documentId}) ?? throw new Exception("Empty result from db.");
        }

        public async Task<ActionResult<List<Order>>> GetByDateAsync(DateTime minDate, DateTime maxDate)
        {
            var sql = "SELECT * FROM Orders as O " +
                      "WHERE O.DocDate > @minDate AND O.DocDate < @maxDate";

            return await _db.QueryAsync<Order>(sql, new {minDate, maxDate}) as List<Order> ?? throw new Exception("Empty result from db.");
        }

        public async Task<ActionResult<List<Order>>> GetByCustomerAsync(Customer customer)
        {
            var sql = "SELECT * FROM ORDERS as O " +
                      "WHERE O.CustomerId = (SELECT TOP(1) Id " +
                      "FROM Customers as C " +
                      "WHERE C.Name = @name)";

            var parameters = new DynamicParameters();
            parameters.Add("@name", customer.Name);

            return await _db.QueryAsync<Order>(sql) as List<Order> ?? throw new Exception("Empty result from db.");
        }

        public async Task<ActionResult> AddAsync(Order order)
        {
            var sql = "INSERT INTO Orders (DocDate, DocumentId, OrderSum, CustomerId" +
                      "VALUES (@docDate, @docId, @orderSum, @customerId)";

            var parameters = new DynamicParameters();
            parameters.Add("@docDate", order.DateTime, DbType.Date, ParameterDirection.Input);
            parameters.Add("@docId", order.DocumentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@orderSum", order.OrderSum, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@customerId", order.Customer.Id, DbType.Int32, ParameterDirection.Input);

            var result = await _db.ExecuteAsync(sql, parameters);
            if(result > 0)
                return new OkResult();

            return new StatusCodeResult(500);
        }

        public async Task<ActionResult> UpdateOrInsertAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> RemoveAsync(int documentId)
        {
            throw new NotImplementedException();
        }
    }
}
