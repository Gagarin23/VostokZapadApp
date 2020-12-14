using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VostokZapadApp.Infrastructure.Data.Initialisation
{
    /// <summary>
    /// Класс нужен для связи между хранимыми процедурами базы и приложением.
    /// </summary>
    class CustomerProcedures
    {
        public static readonly string GetCustomerByName = "GetCustomerByName";
        public static readonly string GetCustomerById = "GetCustomerById";
        public static readonly string AddCustomer = "AddCustomer";
        public static readonly string UpdateOrInsertCustomer = "UpdateOrInsertCustomer";
        public static readonly string RemoveCustomerByName = "RemoveCustomerByName";
        public static readonly string RemoveCustomerById = "RemoveCustomerById";
    }
}
