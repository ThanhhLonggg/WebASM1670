using System.ComponentModel.DataAnnotations;

namespace WebASM1670.ViewModels
{
    public class UploadImageViewModel
    {
        [Display(Name = "Picture")]
        public IFormFile Image { get; set; }
    }
}
