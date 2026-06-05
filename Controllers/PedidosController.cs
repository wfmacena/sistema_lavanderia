using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Data;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Controllers
{
    /// <summary>
    /// Controller responsável pela gestão do ciclo de vida dos pedidos.
    /// Gerencia desde a criação pelo cliente até o processamento e entrega pelo administrador.
    /// </summary>
    public class PedidosController : Controller
    {
        private readonly LavanderiaContext _context;

        public PedidosController(LavanderiaContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Lista os pedidos com filtros por status e nome do cliente.
        /// Clientes veem apenas seus pedidos; Administradores veem todos.
        /// </summary>
        public async Task<IActionResult> Index(string status, string buscaCliente)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            int.TryParse(usuarioIdStr, out int usuarioId);

            var pedidos = _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .AsQueryable();

            if (perfil != "Administrador")
            {
                // Busca o ID do usuário de forma segura da sessão
                var idSessao = HttpContext.Session.GetString("UsuarioId");
                if (int.TryParse(idSessao, out int idLogado))
                {
                    pedidos = pedidos.Where(p => p.UsuarioId == idLogado);
                }
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

        /// <summary>
        /// Exibe os detalhes de um pedido específico, incluindo itens e status.
        /// </summary>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .Include(p => p.Usuario)
                .Include(p => p.ItensPedido)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null) return NotFound();

            // Segurança: usuários comuns só veem seus próprios pedidos
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            int.TryParse(usuarioIdStr, out int usuarioId);

            if (perfil != "Administrador" && pedido.UsuarioId != usuarioId)
                return RedirectToAction("AcessoNegado", "Account");

            return View(pedido);
        }

        /// <summary>
        /// Exibe o formulário de criação de pedido, carregando serviços ativos.
        /// </summary>
        public IActionResult Create()
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            var servicos = _context.Servicos.Where(s => s.Ativo).ToList();

            if (perfil == "Administrador")
            {
                ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nome");
            }
            else
            {
                var clienteIdStr = HttpContext.Session.GetString("ClienteId");
                int.TryParse(clienteIdStr, out int clienteId);
                ViewBag.ClienteId = new SelectList(_context.Clientes.Where(c => c.Id == clienteId), "Id", "Nome", clienteId);
            }

            ViewBag.Servicos = servicos;
            ViewBag.Perfil = perfil;

            var model = new PedidoCreateViewModel
            {              
                DataEntrada = DateTime.Today,
                Status = "Recebido"
            };

            return View(model);
        }

        /// <summary>
        /// Processa a criação de um novo pedido com múltiplos itens.
        /// Calcula automaticamente o valor total com base nos serviços selecionados.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PedidoCreateViewModel model)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");

            if (perfil != "Administrador")
            {
                var clienteIdStr = HttpContext.Session.GetString("ClienteId");
                if (int.TryParse(clienteIdStr, out int clienteId)) model.ClienteId = clienteId;

                var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
                if (int.TryParse(usuarioIdStr, out int usuarioId)) model.UsuarioId = usuarioId;

                model.Status = "Recebido";
            }

            if (model.Itens == null || !model.Itens.Any(i => i.Quantidade > 0))
            {
                ModelState.AddModelError("", "Adicione pelo menos um item ao pedido.");
            }

            if (ModelState.IsValid)
            {
                var pedido = new Pedido
                { 
                    ClienteId = model.ClienteId,
                    UsuarioId = model.UsuarioId,
                    TipoLavagem = model.TipoLavagem,
                    Status = model.Status,
                    DataEntrada = model.DataEntrada,
                    DataEntrega = model.DataEntrega,
                    StatusPagamento = "Pendente",
                    Observacoes = model.Observacoes,
                    Quantidade = model.Itens?.Where(i => i.Quantidade > 0).Sum(i => i.Quantidade) ?? 0,
                    ItensPedido = new List<ItemPedido>()
                };

                decimal valorTotal = 0;
                foreach (var itemVM in (model.Itens ?? new List<ItemPedidoViewModel>()).Where(i => i.Quantidade > 0))
                { 
                    var itemPedido = new ItemPedido
                    {
                        TipoPeca = itemVM.NomeServico,
                        Quantidade = itemVM.Quantidade,
                        ValorUnitario = itemVM.PrecoUnitario,
                        Subtotal = itemVM.PrecoUnitario * itemVM.Quantidade
                    };
                    pedido.ItensPedido.Add(itemPedido);
                    valorTotal += itemPedido.Subtotal;
                }

                pedido.Valor = valorTotal;
                
                _context.Pedidos.Add(pedido);
                await _context.SaveChangesAsync();

                TempData["Sucesso"] = "Pedido realizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Servicos = _context.Servicos.Where(s => s.Ativo).ToList();
            if (perfil == "Administrador")
                ViewBag.ClienteId = new SelectList(_context.Clientes, "Id", "Nome", model.ClienteId);
            else
                ViewBag.ClienteId = new SelectList(_context.Clientes.Where(c => c.Id == model.ClienteId), "Id", "Nome", model.ClienteId);

            return View(model);
        }

        /// <summary>
        /// Altera o status de processamento de um pedido (Apenas Admin).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarStatus(int id, string status)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador")
                return RedirectToAction("AcessoNegado", "Account");

            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();

            pedido.Status = status;

            if (status == "Entregue" && !pedido.DataEntrega.HasValue)
                pedido.DataEntrega = DateTime.Now;

            _context.Update(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Atualiza o status de pagamento e forma de pagamento de um pedido (Apenas Admin).
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlterarStatusPagamento(int id, string status, string formaPagamento)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador")
                return RedirectToAction("AcessoNegado", "Account");

            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();

            pedido.StatusPagamento = status;
            pedido.FormaPagamento = formaPagamento;

            _context.Update(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id)
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            int.TryParse(usuarioIdStr, out int usuarioId);

            var pedido = await _context.Pedidos.FindAsync(id);

            if (pedido == null)
                return NotFound();

            // segurança → cliente só cancela o próprio pedido
            if (pedido.UsuarioId != usuarioId)
                return RedirectToAction("AcessoNegado", "Account");

            // só pode cancelar se ainda não foi entregue
            if (pedido.Status == "Entregue")
                return RedirectToAction(nameof(Index));

            pedido.Status = "Cancelado";

            _context.Update(pedido);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PagamentoPix(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Cliente)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (pedido == null) return NotFound();

            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            int.TryParse(usuarioIdStr, out int usuarioId);

            if (pedido.UsuarioId != usuarioId) return RedirectToAction("AcessoNegado", "Account");

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarPagamentoSimulado(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null) return NotFound();

            pedido.StatusPagamento = "Pago";
            pedido.FormaPagamento = "Pix";
            
            _context.Update(pedido);
            await _context.SaveChangesAsync();

            TempData["Sucesso"] = "Pagamento PIX confirmado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
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
