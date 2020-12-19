using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;
using VostokZapadApp.Domain.Core.DataBase;
using VostokZapadApp.Domain.Interfaces;
using VostokZapadApp.Infrastructure.Data.Initialisation;

namespace VostokZapadApp.Infrastructure.Data
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly IDbConnection _dbConnection;
        private readonly PropertyInfo[] _props = typeof(Customer).GetProperties();

        public CustomerRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task<ActionResult<Customer>> GetAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return new BadRequestResult();
                
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Customer>(CustomerProcedures.GetCustomerByName, new { name });

            if (result != null)
                return result;

            return new NotFoundResult();
        }

        public async Task<ActionResult<Customer>> GetAsync(int id)
        {
            if (id < 1)
                return new BadRequestResult();
            
            var result = await _dbConnection.QueryFirstOrDefaultAsync<Customer>(CustomerProcedures.GetCustomerById, new { id });

            if (result != null)
                return result;

            return new NotFoundResult();
        }

        public async Task<ActionResult<int>> AddAsync(Customer customer)
        {
            if (customer == null)
                return new BadRequestResult();

            #region Cпорная штука, медленная рефлексия

            foreach (var propertyInfo in _props)
            {
                if (propertyInfo.GetValue(customer) == default)
                    return new BadRequestResult();
            }

            #endregion
            
            var parameters = new DynamicParameters();
            parameters.Add("@name", customer.Name, DbType.String, ParameterDirection.Input);

            var id = await _dbConnection.QueryFirstOrDefaultAsync<int>(CustomerProcedures.AddCustomer, parameters);

            if(id != default)
                return new ObjectResult(id) { StatusCode = 201 };

            return new BadRequestResult();
        }

        public async Task<ActionResult> UpdateAsync(Customer customer)
        {
            if (customer == null)
                return new BadRequestResult();

            #region Cпорная штука, медленная рефлексия

            foreach (var propertyInfo in _props)
            {
                if (propertyInfo.GetValue(customer) == default)
                    return new BadRequestResult();
            }

            #endregion
            
            var parameters = new DynamicParameters();
            parameters.Add("@id", customer.Id, DbType.Int32, ParameterDirection.Input);
            parameters.Add("@name", customer.Name, DbType.String, ParameterDirection.Input);

            var updatedName = await _dbConnection.QuerySingleOrDefaultAsync<string>(
                CustomerProcedures.UpdateCustomer, parameters);

            if (customer.Name.Equals(updatedName))
                return new OkResult();
            
            return new NotFoundResult();
        }

        public async Task<ActionResult> RemoveAsync(int id)
        {
            if (id < 1)
                return new BadRequestResult();
            
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.String, ParameterDirection.Input);

            id = await _dbConnection.QuerySingleOrDefaultAsync<int>(CustomerProcedures.RemoveCustomerById, parameters);

            if (id != default)
                return new OkResult();

            return new NotFoundResult();
        }

        public async Task<ActionResult> RemoveAsync(string customerName)
        {
            if (string.IsNullOrWhiteSpace(customerName))
                return new BadRequestResult();
            
            var parameters = new DynamicParameters();
            parameters.Add("@name", customerName, DbType.String, ParameterDirection.Input);

            var name = await _dbConnection.QuerySingleOrDefaultAsync<string>(CustomerProcedures.RemoveCustomerByName, parameters);

            if (name != default)
                return new OkResult();

            return new NotFoundResult();
        }
    }
}
