using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebASM1670.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public int Quantity { get; set; }

        public Book Book { get; set; }

        public string Name { get; set; }

        public int Phone {  get; set; }

        public string Address { get; set; }

    }
}