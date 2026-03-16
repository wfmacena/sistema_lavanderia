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
    }
}