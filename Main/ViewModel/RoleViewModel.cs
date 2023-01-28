using System.ComponentModel.DataAnnotations;

namespace Main.ViewModel
{
    public class RoleViewModel
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string Name { get; set; } = String.Empty;
    }
}
