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

        private decimal ObterValorUnitarioPeca(string tipoPeca)
        {
            return tipoPeca switch
            {
                "Camisa" => 8.00m,
                "Calça" => 10.00m,
                "Jaqueta" => 18.00m,
                "Toalha" => 6.00m,
                "Lençol" => 12.00m,
                "Cobertor" => 25.00m,
                "Edredom" => 35.00m,
                "Outros" => 10.00m,
                _ => 0.00m
            };
        }

        private List<ItemPedido> GerarItensPedido(PedidoCreateViewModel model, int pedidoId)
        {
            var itens = new List<ItemPedido>();

            void AdicionarItem(string tipo, int quantidade)
            {
                if (quantidade > 0)
                {
                    var valorUnitario = ObterValorUnitarioPeca(tipo);

                    itens.Add(new ItemPedido
                    {
                        PedidoId = pedidoId,
                        TipoPeca = tipo,
                        Quantidade = quantidade,
                        ValorUnitario = valorUnitario,
                        Subtotal = valorUnitario * quantidade
                    });
                }
            }

            AdicionarItem("Camisa", model.Camisa);
            AdicionarItem("Calça", model.Calca);
            AdicionarItem("Jaqueta", model.Jaqueta);
            AdicionarItem("Toalha", model.Toalha);
            AdicionarItem("Lençol", model.Lencol);
            AdicionarItem("Cobertor", model.Cobertor);
            AdicionarItem("Edredom", model.Edredom);
            AdicionarItem("Outros", model.Outros);

            return itens;
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

        public IActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");

            if (perfil == "Administrador")
            {
                ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nome");
                ViewBag.NomeClienteLogado = null;
            }
            else
            {
                var clienteIdStr = HttpContext.Session.GetString("ClienteId");
                int.TryParse(clienteIdStr, out int clienteId);

                var cliente = _context.Clientes.FirstOrDefault(c => c.Id == clienteId);

                ViewBag.ClienteId = new SelectList(
                    _context.Clientes.Where(c => c.Id == clienteId),
                    "Id",
                    "Nome",
                    clienteId
                );

                ViewBag.NomeClienteLogado = cliente?.Nome;
            }

            ViewBag.Perfil = perfil;

            var model = new PedidoCreateViewModel
            {
                DataEntrada = DateTime.Today,
                Status = "Recebido"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoCreateViewModel model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var perfil = HttpContext.Session.GetString("UsuarioPerfil");

            if (perfil != "Administrador")
            {
                var clienteIdStr = HttpContext.Session.GetString("ClienteId");
                if (int.TryParse(clienteIdStr, out int clienteId))
                {
                    model.ClienteId = clienteId;
                }

                var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
                if (int.TryParse(usuarioIdStr, out int usuarioId))
                {
                    model.UsuarioId = usuarioId;
                }

                model.Status = "Recebido";
                model.DataEntrega = null;
            }

            int quantidadeTotal =
                model.Camisa + model.Calca + model.Jaqueta + model.Toalha +
                model.Lencol + model.Cobertor + model.Edredom + model.Outros;

            if (quantidadeTotal <= 0)
            {
                ModelState.AddModelError("", "Informe pelo menos uma peça para a solicitação.");
            }

            if (model.DataEntrada == default)
            {
                model.DataEntrada = DateTime.Today;
            }

            if (ModelState.IsValid)
            {
                var pedido = new Pedido
                {
                    ClienteId = model.ClienteId,
                    UsuarioId = model.UsuarioId,
                    TipoLavagem = model.TipoLavagem,
                    Quantidade = quantidadeTotal,
                    Status = model.Status,
                    DataEntrada = model.DataEntrada,
                    DataEntrega = model.DataEntrega,
                    Valor = 0
                };

                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                var itens = GerarItensPedido(model, pedido.Id);
                _context.ItensPedido.AddRange(itens);

                pedido.Valor = itens.Sum(i => i.Subtotal);

                _context.Pedidos.Update(pedido);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            if (perfil == "Administrador")
            {
                ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nome", model.ClienteId);
                ViewBag.NomeClienteLogado = null;
            }
            else
            {
                var cliente = _context.Clientes.FirstOrDefault(c => c.Id == model.ClienteId);

                ViewBag.ClienteId = new SelectList(
                    _context.Clientes.Where(c => c.Id == model.ClienteId),
                    "Id",
                    "Nome",
                    model.ClienteId
                );

                ViewBag.NomeClienteLogado = cliente?.Nome;
            }

            ViewBag.Perfil = perfil;
            return View(model);
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