using System.ComponentModel.DataAnnotations;

namespace SistemaLavanderia.Models
{
    public class PedidoCreateViewModel
    {
        public int ClienteId { get; set; }

        public int? UsuarioId { get; set; }

        [Required(ErrorMessage = "O tipo de lavagem é obrigatório.")]
        public string TipoLavagem { get; set; } = string.Empty;

        [Required]
        public string Status { get; set; } = "Recebido";

        [DataType(DataType.Date)]
        public DateTime DataEntrada { get; set; } = DateTime.Today;

        [DataType(DataType.Date)]
        public DateTime? DataEntrega { get; set; }

        [Range(0, 100)]
        public int Camisa { get; set; }

        [Range(0, 100)]
        public int Calca { get; set; }

        [Range(0, 100)]
        public int Jaqueta { get; set; }

        [Range(0, 100)]
        public int Toalha { get; set; }

        [Range(0, 100)]
        public int Lencol { get; set; }

        [Range(0, 100)]
        public int Cobertor { get; set; }

        [Range(0, 100)]
        public int Edredom { get; set; }

        [Range(0, 100)]
        public int Outros { get; set; }
    }
}