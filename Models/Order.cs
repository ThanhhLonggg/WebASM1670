using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebASM1670.Models
{
    [Table("Orders")]
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public double TotalPrice { get; set; }

        public int UserId { get; set; }

        public List<OrderItem> Items { get; set; }

    }
}
