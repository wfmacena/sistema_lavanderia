using System.ComponentModel.DataAnnotations;

namespace SistemaLavanderia.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O login é obrigatório.")]
        [StringLength(50)]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100)]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O perfil é obrigatório.")]
        [StringLength(20)]
        public string Perfil { get; set; } = "Usuario";

        [Phone(ErrorMessage = "Telefone inválido.")]
        [StringLength(20)]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(14)]
        public string Cpf { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Email inválido.")]
        [StringLength(100)]
        public string? Email { get; set; }

        public int? ClienteId { get; set; }
        public Cliente? Cliente { get; set; }
    }
}