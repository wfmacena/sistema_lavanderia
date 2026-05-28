using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaLavanderia.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public int ClienteId { get; set; }

        public int? UsuarioId { get; set; }

        [Required(ErrorMessage = "O tipo de lavagem é obrigatório.")]
        [StringLength(50)]
        public string TipoLavagem { get; set; } = string.Empty;

        [Required(ErrorMessage = "A quantidade é obrigatória.")]
        [Range(1, 1000)]
        public int Quantidade { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        [Range(0, 999999.99)]
        public decimal Valor { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Recebido";

        [Required]
        [StringLength(20)]
        public string StatusPagamento { get; set; } = "Pendente";

        [StringLength(50)]
        public string? FormaPagamento { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataEntrada { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? DataEntrega { get; set; }

        public string? Observacoes { get; set; }

        public Cliente? Cliente { get; set; }
        public Usuario? Usuario { get; set; }
        public List<ItemPedido> ItensPedido { get; set; } = new();
    }
}