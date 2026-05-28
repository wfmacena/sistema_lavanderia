namespace SistemaLavanderia.Models
{
    public class DashboardViewModel
    {
        public int TotalClientes { get; set; }
        public int TotalPedidos { get; set; }
        public int PedidosRecebidos { get; set; }
        public int PedidosEmLavagem { get; set; }
        public int PedidosProntos { get; set; }
        public int PedidosEntregues { get; set; }
        
        // Novas métricas profissionais
        public decimal FaturamentoPago { get; set; }
        public decimal FaturamentoPendente { get; set; }
        public int TotalServicosAtivos { get; set; }
    }
}
