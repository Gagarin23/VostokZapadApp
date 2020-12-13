using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace VostokZapadApp.Domain.Core
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public DateTime DateTime { get; set; }
        [Required]
        public int DocumentId { get; set; } //с залогом на то, что может быть ещё некий документ отдельно хранящий
                                            //информацию заказа(счёт/товарная накладная/акт и тд...)

        [Range(typeof(decimal), "0", "9999999999999999999999", ErrorMessage = "")] 
        public decimal Sum { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public Customer Customer { get; set; }
    }
}
