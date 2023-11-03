using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using WebASM1670.Models;

namespace WebASM1670.ViewModels
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }

        public int BookId { get; set; }

        public int Quantity { get; set; }

        public string Name { get; set; }

        public int Phone { get; set; }

        public string Address { get; set; }
        [ValidateNever]
        public List<Book> Books { get; set; }
    }
}
