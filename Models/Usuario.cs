using System.ComponentModel.DataAnnotations;

namespace SistemaLavanderia.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Login { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Senha { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Perfil { get; set; } = "Usuario";
    }
}