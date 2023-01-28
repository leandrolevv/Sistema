using System.ComponentModel.DataAnnotations;

namespace Main.ViewModel.EditorViewModel
{
    public class EditorRoleViewModel
    {
        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;
    }
}
