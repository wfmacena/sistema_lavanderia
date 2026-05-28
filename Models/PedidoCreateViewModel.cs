using System.ComponentModel.DataAnnotations;

namespace SistemaLavanderia.Models
{
    public class PedidoCreateViewModel
    {
        public int ClienteId { get; set; }
        public int? UsuarioId { get; set; }

        [Required(ErrorMessage = "O tipo de lavagem é obrigatório.")]
        public string TipoLavagem { get; set; } = "Padrão";

        [Required]
        public string Status { get; set; } = "Recebido";

        [DataType(DataType.Date)]
        public DateTime DataEntrada { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? DataEntrega { get; set; }

        public string? Observacoes { get; set; }

        // Lista de itens selecionados (JSON ou lista de objetos)
        public List<ItemPedidoViewModel> Itens { get; set; } = new List<ItemPedidoViewModel>();
    }

    public class ItemPedidoViewModel
    {
        public int ServicoId { get; set; }
        public string NomeServico { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal PrecoUnitario { get; set; }
    }
}
