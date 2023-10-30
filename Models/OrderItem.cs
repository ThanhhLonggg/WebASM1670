using System.ComponentModel.DataAnnotations.Schema;

namespace WebASM1670.Models
{
    [Table("OrderItems")]
    public class OrderItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }
    }
}
