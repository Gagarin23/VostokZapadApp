using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace VostokZapadApp.Domain.Core
{
    public class Sales
    {
        public Customer Customer { get; set; }
        public List<Order> Orders { get; set; }
    }
}
