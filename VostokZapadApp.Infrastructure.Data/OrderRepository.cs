using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
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
        private readonly IDbConnection _dbConnection;

        public OrderRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ActionResult<List<Order>>> GetAllAsync()
        {
            var orders = await _dbConnection.QueryAsync<Order>(OrderProcedures.GetAllOrders, commandType: CommandType.StoredProcedure)
                             as List<Order> ?? new List<Order>();

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<Order>> GetByDocIdAsync(int documentId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DocId", documentId, DbType.Int32, ParameterDirection.Input);

            var orders = await _dbConnection.QueryFirstAsync<Order>(OrderProcedures.GetAllOrders,
                commandType: CommandType.StoredProcedure);

            if (orders == null)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<List<Order>>> GetByDateAsync(DateTime minDate, DateTime maxDate)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@MinDate", minDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);
            parameters.Add("@MaxDate", maxDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);

            var orders = await _dbConnection.QueryAsync<Order>(OrderProcedures.GetAllOrders, commandType: CommandType.StoredProcedure)
                as List<Order> ?? new List<Order>();

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<List<Order>>> GetByCustomerAsync(Customer customer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Name", customer.Name, DbType.String, ParameterDirection.Input);

            var orders = await _dbConnection.QueryAsync<Order>(OrderProcedures.GetAllOrders, commandType: CommandType.StoredProcedure)
                as List<Order> ?? new List<Order>();

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<int>> AddAsync(Order order)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DocDate", order.DocDate, DbType.Date, ParameterDirection.Input);
            parameters.Add("@DocId", order.DocumentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@OrderSum", order.OrderSum, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@CustomerId", order.CustomerId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@statusCode", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            var id = await _dbConnection.QueryFirstOrDefaultAsync<int>(
                OrderProcedures.AddOrder, parameters, commandType: CommandType.StoredProcedure);

            return new ObjectResult(id) { StatusCode = parameters.Get<int>("@statusCode") };
        }

        public async Task<ActionResult> UpdateOrInsertAsync(Order order)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@docDate", order.DocDate, DbType.Date, ParameterDirection.Input);
            parameters.Add("@docId", order.DocumentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@orderSum", order.OrderSum, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@customerId", order.CustomerId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@statusCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            var result = await _dbConnection.ExecuteAsync(OrderProcedures.UpdateOrder, parameters, commandType: CommandType.StoredProcedure);

            return new StatusCodeResult(result);
        }

        public async Task<ActionResult> RemoveAsync(int documentId)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@documentId", documentId, DbType.String, ParameterDirection.Input);
            parameters.Add("@statusCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _dbConnection.ExecuteAsync(OrderProcedures.RemoveOrder, parameters, commandType: CommandType.StoredProcedure);

            return new StatusCodeResult(parameters.Get<int>("@statusCode"));
        }
    }
}
