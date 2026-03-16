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

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("UsuarioLogin") == null)
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            var nome = HttpContext.Session.GetString("UsuarioNome");
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            int.TryParse(usuarioIdStr, out int usuarioId);

            ViewBag.Nome = nome;
            ViewBag.Perfil = perfil;

            if (perfil == "Administrador")
            {
                var pedidosRecentes = _context.Pedidos
                    .Include(p => p.Cliente)
                    .OrderByDescending(p => p.DataEntrada)
                    .Take(5)
                    .ToList();

                ViewBag.PedidosRecentes = pedidosRecentes;
            }
            else
            {
                var meusPedidos = _context.Pedidos
                    .Include(p => p.Cliente)
                    .Where(p => p.UsuarioId == usuarioId)
                    .OrderByDescending(p => p.DataEntrada)
                    .Take(5)
                    .ToList();

                ViewBag.MeusPedidos = meusPedidos;
            }

            return View();
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