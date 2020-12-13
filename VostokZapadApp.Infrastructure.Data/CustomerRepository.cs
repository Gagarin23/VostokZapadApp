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
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _dbConnection;

        public CustomerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ActionResult<Customer>> GetAsync(string name)
        {
            var sql = "SELECT TOP(1) Id, Name FROM Customers " +
                      "WHERE Name = @name";

            return await _dbConnection.QueryFirstOrDefaultAsync<Customer>(sql, new {name});
        }

        public async Task<ActionResult> AddOrUpdateAsync(Customer customer)
        {
            var sql = "INSERT INTO Customers (Name) " +
                      "VALUES (@name)";


            var parameters = new DynamicParameters();
            parameters.Add("@name", customer.Name, DbType.String, ParameterDirection.Input);

            var rows = await _dbConnection.ExecuteAsync(sql, parameters);
            if(rows > 0)
                return new OkResult();

            return new StatusCodeResult(500);
        }

        public async Task<ActionResult> UpdateOrInsertAsync(Customer customer)
        {
            throw new NotImplementedException();
        }

        public async Task<ActionResult> RemoveAsync(string customerName)
        {
            throw new NotImplementedException();
        }
    }
}
