using System.Collections.Generic;
using VostokZapadApp.Domain.Core.DataBase;

namespace VostokZapadApp.Domain.Core.InputOutputData
{
    public class Sales
    {
        public Customer Customer { get; set; }
        public List<Order> Orders { get; set; }
    }
}
