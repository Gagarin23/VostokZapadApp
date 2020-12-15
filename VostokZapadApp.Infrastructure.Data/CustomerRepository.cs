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
using VostokZapadApp.Infrastructure.Data.Initialisation;

namespace VostokZapadApp.Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _dbConnection;

        public CustomerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ActionResult<Customer>> GetAsync(string name)
        {
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Customer>(CustomerProcedures.GetCustomerByName, new {name}, 
                commandType: CommandType.StoredProcedure);

            if (result != null)
                return result;

            return new NotFoundResult();
        }

        public async Task<ActionResult<Customer>> GetAsync(int id)
        {
            var result =  await _dbConnection.QueryFirstOrDefaultAsync<Customer>(CustomerProcedures.GetCustomerById, new {id},
                commandType: CommandType.StoredProcedure);

            if (result != null)
                return result;

            return new NotFoundResult();
        }

        public async Task<ActionResult<int>> AddAsync(Customer customer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@name", customer.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@statusCode", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            var id = await _dbConnection.QueryFirstOrDefaultAsync<int>(CustomerProcedures.AddCustomer, parameters,
                commandType: CommandType.StoredProcedure);

            return new ObjectResult(id) {StatusCode = parameters.Get<int>("@statusCode")};
        }

        public async Task<ActionResult> UpdateAsync(Customer customer)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", customer.Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@name", customer.Name, DbType.String, ParameterDirection.Input);
            parameters.Add("@statusCode", dbType: DbType.String, direction: ParameterDirection.ReturnValue);

            await _dbConnection.ExecuteAsync(
                CustomerProcedures.UpdateCustomer, parameters, commandType:CommandType.StoredProcedure);

            return new StatusCodeResult(parameters.Get<int>("@statusCode"));
        }

        public async Task<ActionResult> RemoveAsync(int id)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.String, ParameterDirection.Input);
            parameters.Add("@statusCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _dbConnection.ExecuteAsync(CustomerProcedures.RemoveCustomerById, parameters, commandType: CommandType.StoredProcedure);

            return new StatusCodeResult(parameters.Get<int>("@statusCode"));
        }

        public async Task<ActionResult> RemoveAsync(string customerName)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@name", customerName, DbType.String, ParameterDirection.Input);
            parameters.Add("@statusCode", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);

            await _dbConnection.ExecuteAsync(CustomerProcedures.RemoveCustomerByName, parameters, commandType: CommandType.StoredProcedure);

            return new StatusCodeResult(parameters.Get<int>("@statusCode"));
        }
    }
}
