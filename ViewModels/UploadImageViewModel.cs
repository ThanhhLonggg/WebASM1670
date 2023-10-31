using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace WebASM1670.ViewModels
{
    public class UploadImageViewModel
    {
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }
    }
}
