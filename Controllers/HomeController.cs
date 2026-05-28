using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Data;
using SistemaLavanderia.Models;
using System.Diagnostics;

namespace SistemaLavanderia.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly LavanderiaContext _context;

        public HomeController(ILogger<HomeController> logger, LavanderiaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Welcome()
        {
            return View();
        }

        public async Task<IActionResult> Index()
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            var nome = HttpContext.Session.GetString("UsuarioNome");
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            int.TryParse(usuarioIdStr, out int usuarioId);

            ViewBag.Nome = nome;
            ViewBag.Perfil = perfil;

            var dashboard = new DashboardViewModel
            {
                TotalClientes = await _context.Clientes.CountAsync(),
                TotalPedidos = await _context.Pedidos.CountAsync(),
                PedidosRecebidos = await _context.Pedidos.CountAsync(p => p.Status == "Recebido"),
                PedidosEmLavagem = await _context.Pedidos.CountAsync(p => p.Status == "Lavando" || p.Status == "Secando" || p.Status == "Passando"),
                PedidosProntos = await _context.Pedidos.CountAsync(p => p.Status == "Pronto"),
                PedidosEntregues = await _context.Pedidos.CountAsync(p => p.Status == "Entregue"),
                TotalServicosAtivos = await _context.Servicos.CountAsync(s => s.Ativo)
            };

            if (perfil == "Administrador")
            {
                ViewBag.PedidosRecentes = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .OrderByDescending(p => p.DataEntrada)
                    .Take(5)
                    .ToListAsync();
                
                var listaFaturamentoPago = await _context.Pedidos
                    .Where(p => p.StatusPagamento == "Pago")
                    .Select(p => p.Valor)
                    .ToListAsync();
                
                var listaFaturamentoPendente = await _context.Pedidos
                    .Where(p => p.StatusPagamento != "Pago" && p.Status != "Cancelado")
                    .Select(p => p.Valor)
                    .ToListAsync();

                dashboard.FaturamentoPago = listaFaturamentoPago.Sum();
                dashboard.FaturamentoPendente = listaFaturamentoPendente.Sum();
                
                ViewBag.FaturamentoTotal = dashboard.FaturamentoPago;
            }
            else
            {
                ViewBag.MeusPedidos = await _context.Pedidos
                    .Include(p => p.Cliente)
                    .Where(p => p.UsuarioId == usuarioId)
                    .OrderByDescending(p => p.DataEntrada)
                    .Take(5)
                    .ToListAsync();

                dashboard.TotalPedidos = await _context.Pedidos.CountAsync(p => p.UsuarioId == usuarioId);
                dashboard.PedidosRecebidos = await _context.Pedidos.CountAsync(p => p.UsuarioId == usuarioId && p.Status == "Recebido");
                dashboard.PedidosEmLavagem = await _context.Pedidos.CountAsync(p => p.UsuarioId == usuarioId && (p.Status == "Lavando" || p.Status == "Secando" || p.Status == "Passando"));
                dashboard.PedidosProntos = await _context.Pedidos.CountAsync(p => p.UsuarioId == usuarioId && p.Status == "Pronto");
                dashboard.PedidosEntregues = await _context.Pedidos.CountAsync(p => p.UsuarioId == usuarioId && p.Status == "Entregue");
            }

            return View(dashboard);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
