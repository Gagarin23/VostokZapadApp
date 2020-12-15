using System;
using System.ComponentModel.DataAnnotations;

namespace VostokZapadApp.Domain.Core.DataBase
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        public DateTime DocDate { get; set; }
        public int DocumentId { get; set; } //todo: надо было делать строкой. UPD Ну или добавить столбец и сделать составной ключ.
        public decimal OrderSum { get; set; }
        public int CustomerId { get; set; }
    }
}
