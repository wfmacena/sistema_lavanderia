namespace SistemaLavanderia.Models
{
    public class ServicoViewModel
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal PrecoBase { get; set; }
        public string UnidadeMedida { get; set; } = string.Empty;
    }
}