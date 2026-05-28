using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaLavanderia.Models
{
    public class Servico
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do serviço é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty;

        [Required(ErrorMessage = "O preço base é obrigatório.")]
        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999.99)]
        public decimal PrecoBase { get; set; }

        [Required(ErrorMessage = "A unidade de medida é obrigatória.")]
        [StringLength(20)]
        public string UnidadeMedida { get; set; } = "Peça"; // Peça, Kg, Par, etc.

        public bool Ativo { get; set; } = true;
    }
}
