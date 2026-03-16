using System.ComponentModel.DataAnnotations;

namespace SistemaLavanderia.Models
{
    public class LoginViewModel
    {
        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Senha { get; set; } = string.Empty;
    }
}