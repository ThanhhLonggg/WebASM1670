using WebASM1670.Models;
using System.ComponentModel.DataAnnotations;

namespace WebASM1670.ViewModels
{
    public class BookModel : EditImageViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public IFormFile Image { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public List<Category> Categories { get; set; }

        public List<CartItem> CartItems { get; set; }
    }
}
