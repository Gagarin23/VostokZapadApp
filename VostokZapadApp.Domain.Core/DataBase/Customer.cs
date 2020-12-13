using System.ComponentModel.DataAnnotations;

namespace VostokZapadApp.Domain.Core.DataBase
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
