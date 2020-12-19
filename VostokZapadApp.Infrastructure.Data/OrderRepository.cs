using Dapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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
        private readonly PropertyInfo[] _props = typeof(Order).GetProperties();

        public OrderRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ActionResult<List<Order>>> GetAllAsync()
        {
            var orders = await _dbConnection.QueryAsync<Order>(OrderProcedures.GetAllOrders)
                             as List<Order> ?? new List<Order>();

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<Order>> GetByDocIdAsync(int documentId)
        {
            if (documentId < 1)
                return new BadRequestResult();
            
            var parameters = new DynamicParameters();
            parameters.Add("@DocId", documentId, DbType.Int32, ParameterDirection.Input);

            var order = await _dbConnection.QueryFirstOrDefaultAsync<Order>(OrderProcedures.GetOrderByDocumentId, parameters);

            if (order == null)
                return new NotFoundResult();

            return order;
        }

        public async Task<ActionResult<List<Order>>> GetByDateAsync(DateTime minDate, DateTime maxDate)
        {
            if (minDate > maxDate)
                return new BadRequestResult();
            
            var parameters = new DynamicParameters();
            parameters.Add("@MinDate", minDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);
            parameters.Add("@MaxDate", maxDate.ToString("yyyy-MM-dd"), DbType.Date, ParameterDirection.Input);

            var orders = await _dbConnection.QueryAsync<Order>(OrderProcedures.GetOrdersByDate, parameters)
                as List<Order> ?? new List<Order>();

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<List<Order>>> GetByCustomerAsync(Customer customer)
        {
            if (customer == null || customer.Name == null)
                return new BadRequestResult();
            
            var parameters = new DynamicParameters();
            parameters.Add("@Name", customer.Name, DbType.String, ParameterDirection.Input);

            var orders = await _dbConnection.QueryAsync<Order>(OrderProcedures.GetOrdersByCustomer, parameters)
                as List<Order> ?? new List<Order>();

            if (orders.Count < 1)
                return new NotFoundResult();

            return orders;
        }

        public async Task<ActionResult<int>> AddAsync(Order order)
        {
            if (order == null)
                return new BadRequestResult();
            
            #region Cпорная штука, медленная рефлексия. 
            
            foreach (var propertyInfo in _props)
            {
                if (propertyInfo.GetValue(order) == default)
                    return new BadRequestResult();
            }
            
            #endregion
            
            var parameters = new DynamicParameters();
            parameters.Add("@DocDate", order.DocDate, DbType.Date, ParameterDirection.Input);
            parameters.Add("@DocId", order.DocumentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@OrderSum", order.OrderSum, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@CustomerId", order.CustomerId, DbType.Int32, ParameterDirection.Input);

            var id = await _dbConnection.QueryFirstOrDefaultAsync<int>(OrderProcedures.AddOrder, parameters);

            if(id != default)
                return new ObjectResult(id) { StatusCode = 201 };

            return new BadRequestResult();
        }

        public async Task<ActionResult> UpdateOrInsertAsync(Order order)
        {            
            if (order == null)
                return new BadRequestResult();
            
            #region Cпорная штука, медленная рефлексия. 
            
            foreach (var propertyInfo in _props)
            {
                if (propertyInfo.GetValue(order) == default)
                    return new BadRequestResult();
            }
            
            #endregion
            
            var parameters = new DynamicParameters();
            parameters.Add("@docDate", order.DocDate, DbType.Date, ParameterDirection.Input);
            parameters.Add("@docId", order.DocumentId, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@orderSum", order.OrderSum, DbType.Decimal, ParameterDirection.Input);
            parameters.Add("@customerId", order.CustomerId, DbType.Int32, ParameterDirection.Input);

            var updOrder = await _dbConnection.QuerySingleOrDefaultAsync<Order>(OrderProcedures.UpdateOrder, parameters);

            if (order.DocDate == updOrder.DocDate
            && order.CustomerId == updOrder.CustomerId
            && order.DocumentId == updOrder.DocumentId
            && order.OrderSum == updOrder.OrderSum)
                return new OkResult();

            return new NotFoundResult();
        }

        public async Task<ActionResult> RemoveAsync(int documentId)
        {
            if (documentId < 1)
                return new BadRequestResult();
            
            var parameters = new DynamicParameters();
            parameters.Add("@documentId", documentId, DbType.String, ParameterDirection.Input);

            var id = await _dbConnection.QuerySingleOrDefaultAsync<int>(OrderProcedures.RemoveOrder, parameters);

            if (id != default)
                return new OkResult();

            return new NotFoundResult();
        }
    }
}
