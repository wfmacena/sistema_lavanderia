using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Data;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Controllers
{
    public class ClientesController : Controller
    {
        private readonly LavanderiaContext _context;

        public ClientesController(LavanderiaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Clientes.ToListAsync());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Cliente cliente)
        {
            if (id != cliente.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente != null)
                {
                    // Verifica se o cliente tem pedidos antes de excluir
                    var temPedidos = await _context.Pedidos.AnyAsync(p => p.ClienteId == id);
                    if (temPedidos)
                    {
                        TempData["Erro"] = "Não é possível excluir este cliente pois ele possui pedidos registrados. Tente desativar o usuário vinculado em vez de excluir.";
                        return RedirectToAction(nameof(Index));
                    }

                    _context.Clientes.Remove(cliente);
                    await _context.SaveChangesAsync();
                    TempData["Sucesso"] = "Cliente excluído com sucesso!";
                }
            }
            catch (Exception)
            {
                TempData["Erro"] = "Ocorreu um erro ao tentar excluir o cliente. Verifique se existem dependências.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}