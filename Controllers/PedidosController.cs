using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Data;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Controllers
{
    public class PedidosController : Controller
    {
        private readonly LavanderiaContext _context;

        public PedidosController(LavanderiaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string status, string buscaCliente)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            int.TryParse(usuarioIdStr, out int usuarioId);

            var pedidos = _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .AsQueryable();

            if (perfil != "Administrador")
            {
                pedidos = pedidos.Where(p => p.UsuarioId == usuarioId);
            }

            if (!string.IsNullOrEmpty(status))
            {
                pedidos = pedidos.Where(p => p.Status == status);
            }

            if (!string.IsNullOrEmpty(buscaCliente) && perfil == "Administrador")
            {
                pedidos = pedidos.Where(p => p.Cliente != null && p.Cliente.Nome.Contains(buscaCliente));
            }

            ViewBag.StatusAtual = status;
            ViewBag.BuscaCliente = buscaCliente;
            ViewBag.Perfil = perfil;

            return View(await pedidos.ToListAsync());
        }

        private decimal CalcularValor(string tipoLavagem, int quantidade)
        {
            decimal precoUnitario = tipoLavagem switch
            {
                "Lavagem Comum" => 10.00m,
                "Lavagem a Seco" => 20.00m,
                "Edredom" => 35.00m,
                _ => 0.00m
            };

            return precoUnitario * quantidade;
        }

        public IActionResult Create()
        {
            ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pedido pedido)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            if (ModelState.IsValid)
            {
                pedido.Valor = CalcularValor(pedido.TipoLavagem, pedido.Quantidade);

                var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
                if (int.TryParse(usuarioIdStr, out int usuarioId))
                {
                    pedido.UsuarioId = usuarioId;
                }

                _context.Add(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nome", pedido.ClienteId);
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarComoEntregue(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador")
                return RedirectToAction("AcessoNegado", "Account");

            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();

            pedido.Status = "Entregue";

            if (!pedido.DataEntrega.HasValue)
                pedido.DataEntrega = DateTime.Now;

            _context.Update(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador")
                return RedirectToAction("AcessoNegado", "Account");

            if (id == null) return NotFound();

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nome", pedido.ClienteId);
            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pedido pedido)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador")
                return RedirectToAction("AcessoNegado", "Account");

            if (id != pedido.Id) return NotFound();

            if (ModelState.IsValid)
            {
                pedido.Valor = CalcularValor(pedido.TipoLavagem, pedido.Quantidade);

                _context.Update(pedido);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nome", pedido.ClienteId);
            return View(pedido);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador")
                return RedirectToAction("AcessoNegado", "Account");

            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador")
                return RedirectToAction("AcessoNegado", "Account");

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}