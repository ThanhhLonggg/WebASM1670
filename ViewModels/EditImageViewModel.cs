using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebASM1670.ViewModels
{
    public class EditImageViewModel: UploadImageViewModel
    {
        public int Id { get; set; }
        [ValidateNever]
        public string? ExistingImage { get; set; }
    }
}
