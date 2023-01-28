using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Main.Models;

namespace Main.ViewModel
{
    public class EditorUserViewModel
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        [MinLength(8, ErrorMessage = "O campo nome deve ter ao menos 8 caracteres.")]
        public string Name { get; set; } = String.Empty;
        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O campo Email não representa um email válido.")]
        public string Email { get; set; } = String.Empty;
        [Required(ErrorMessage = "O campo Senha é obrigatório.")]
        [MinLength(8, ErrorMessage = "A Senha deve ter ao menos 8 caracteres.")]
        public string PasswordHash { get; set; } = String.Empty;
        
        //public List<Role> Roles { get; set; } = new List<Role>(); /*Criar primeiro role antes de tornar obrigatório*/
    }
}
