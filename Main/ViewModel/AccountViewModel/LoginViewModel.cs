using System.ComponentModel.DataAnnotations;

namespace Main.ViewModel.AccountViewModel
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email não representa um email válido.")]
        public string Email { get; set; } = string.Empty;
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [MinLength(8, ErrorMessage = "A Senha deve ter ao menos 8 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
}
