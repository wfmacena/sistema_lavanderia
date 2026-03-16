using System.ComponentModel.DataAnnotations;

namespace SistemaLavanderia.Models
{
    public class ConfiguracoesContaViewModel
    {
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Email inválido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O telefone é obrigatório.")]
        [StringLength(15)]
        public string Telefone { get; set; } = string.Empty;
    }
}