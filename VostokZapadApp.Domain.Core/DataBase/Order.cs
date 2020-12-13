using System;
using System.ComponentModel.DataAnnotations;

namespace VostokZapadApp.Domain.Core.DataBase
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public int DocumentId { get; set; } 
        public decimal OrderSum { get; set; }
        public int CustomerId { get; set; }
    }
}
