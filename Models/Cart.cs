using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebASM1670.Models
{
    [Table("Carts")]
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        public int Quantity { get; set; }

        public double TotalPrice { get; set; }

        public List<CartItem> Items { get; set; }
    }
}
