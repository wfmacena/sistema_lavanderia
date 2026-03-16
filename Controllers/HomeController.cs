using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var viewModel = new DashboardViewModel
            {
                TotalClientes = _context.Clientes.Count(),
                TotalPedidos = _context.Pedidos.Count(),
                PedidosRecebidos = _context.Pedidos.Count(p => p.Status == "Recebido"),
                PedidosEmLavagem = _context.Pedidos.Count(p => p.Status == "Em Lavagem"),
                PedidosProntos = _context.Pedidos.Count(p => p.Status == "Pronto"),
                PedidosEntregues = _context.Pedidos.Count(p => p.Status == "Entregue")
            };

            return View(viewModel);
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