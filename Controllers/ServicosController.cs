using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Data;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Controllers
{
    public class ServicosController : Controller
    {
        private readonly LavanderiaContext _context;

        public ServicosController(LavanderiaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var servicos = await _context.Servicos.Where(s => s.Ativo).ToListAsync();
            return View(servicos);
        }

        public IActionResult Create()
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador")
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Servico servico)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador") return RedirectToAction("Index");

            if (ModelState.IsValid)
            {
                _context.Add(servico);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(servico);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador") return RedirectToAction("Index");

            if (id == null) return NotFound();

            var servico = await _context.Servicos.FindAsync(id);
            if (servico == null) return NotFound();

            return View(servico);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Servico servico)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador") return RedirectToAction("Index");

            if (id != servico.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(servico);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Servicos.Any(e => e.Id == servico.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(servico);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador") return RedirectToAction("Index");

            if (id == null) return NotFound();

            var servico = await _context.Servicos.FirstOrDefaultAsync(m => m.Id == id);
            if (servico == null) return NotFound();

            return View(servico);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var perfil = HttpContext.Session.GetString("UsuarioPerfil");
            if (perfil != "Administrador") return RedirectToAction("Index");

            var servico = await _context.Servicos.FindAsync(id);
            if (servico != null)
            {
                servico.Ativo = false; // Soft delete
                _context.Update(servico);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
