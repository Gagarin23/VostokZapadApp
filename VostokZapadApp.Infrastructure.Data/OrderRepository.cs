using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using VostokZapadApp.Domain.Core;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Interfaces;
using VostokZapadApp.Infrastructure.Data.Initialisation;

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
            var reader = await _db.ExecuteReaderAsync(OrderProcedures.GetAllOrders, commandType: CommandType.StoredProcedure) 
                             as DbDataReader ?? throw new Exception("Ошибка каста IDbDataReader в DbDataReader");

            var orders = GetOrders(reader);

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<Order>> GetByDocIdAsync(int documentId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DocId", documentId, DbType.Int32, ParameterDirection.Input);

            var reader = await _db.ExecuteReaderAsync(
                OrderProcedures.GetOrderByDocumentId, parameters, commandType: CommandType.StoredProcedure) as DbDataReader ??
                         throw new Exception("Ошибка каста IDbDataReader в DbDataReader");;

            var orders = GetOrders(reader).FirstOrDefault();

            if (orders == null)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<List<Order>>> GetByDateAsync(DateTime minDate, DateTime maxDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@MinDate", minDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);
            parameters.Add("@MaxDate", maxDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);

            var reader = await _db.ExecuteReaderAsync(
                OrderProcedures.GetOrdersByDate, parameters, commandType: CommandType.StoredProcedure) as DbDataReader ??
                throw new Exception("Ошибка каста IDbDataReader в DbDataReader");

            var orders = GetOrders(reader);

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<List<Order>>> GetByCustomerAsync(Customer customer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Name", customer.Name, DbType.String, ParameterDirection.Input);

            var reader = await _db.ExecuteReaderAsync(
                    OrderProcedures.GetOrdersByCustomer, parameters, commandType:CommandType.StoredProcedure) as DbDataReader ??
                         throw new Exception("Ошибка каста IDbDataReader в DbDataReader");

            var orders = GetOrders(reader);

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult> AddAsync(Order order)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DocDate", order.DateTime, DbType.Date, ParameterDirection.Input);
            parameters.Add("@DocId", order.DocumentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@OrderSum", order.OrderSum, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@CustomerId", order.CustomerId, DbType.Int32, ParameterDirection.Input);

            var result = await _db.ExecuteAsync(
                OrderProcedures.AddOrder, parameters, commandType: CommandType.StoredProcedure);

            if(result > 0)
                return new StatusCodeResult(201);

            return new StatusCodeResult(500);
        }

        public async Task<ActionResult> UpdateOrInsertAsync(Order order) //todo: поменять этого монстра на обычный update 200 или 404
        {
            var sql = "MERGE " +
                      "INTO GetOrders WITH (HOLDLOCK) AS target " +
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

        //todo: доделать удаление заказа если успею.
        public async Task<ActionResult> RemoveAsync(int documentId)
        {
            throw new NotImplementedException();
        }

        private List<Order> GetOrders(DbDataReader reader)
        {
            var orders = new List<Order>();

            while (reader.HasRows)
            {
                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        Id = reader.GetInt32(0),
                        DateTime = reader.GetDateTime(1),
                        DocumentId = reader.GetInt32(2),
                        OrderSum = reader.GetDecimal(3),
                        CustomerId = reader.GetInt32(4)
                    });
                }

                reader.NextResult();
            }

            return orders;
        }
    }
}
