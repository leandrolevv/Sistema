using System.ComponentModel.DataAnnotations;

namespace Main.ViewModel.UserViewModel
{
    public class ChangePicUserViewModel
    {
        [Required]
        public String Base64Image { get; set; } = String.Empty;
    }
}
