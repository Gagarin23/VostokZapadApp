using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VostokZapadApp.Infrastructure.Data.Initialisation
{
    public class OrderProcedures
    {
        public static readonly string GetAllOrders = "GetAllOrders";
        public static readonly string GetOrderByDocumentId = "GetOrderByDocumentId";
        public static readonly string GetOrdersByDate = "GetOrdersByDate";
        public static readonly string GetOrdersByCustomer = "GetOrdersByCustomer";
        public static readonly string AddOrder = "AddOrder";
        public static readonly string UpdateOrder = "UpdateOrder";
        public static readonly string RemoveOrder = "RemoveOrder";
    }
}
