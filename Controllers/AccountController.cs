using Microsoft.AspNetCore.Mvc;
using SistemaLavanderia.Data;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Controllers
{
    public class AccountController : Controller
    {
        private readonly LavanderiaContext _context;

        public AccountController(LavanderiaContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Login == model.Login && u.Senha == model.Senha);

            if (usuario == null)
            {
                ViewBag.Erro = "Login ou senha inválidos.";
                return View(model);
            }

            HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());
            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
            HttpContext.Session.SetString("UsuarioPerfil", usuario.Perfil);
            HttpContext.Session.SetString("UsuarioLogin", usuario.Login);

            if (usuario.ClienteId.HasValue)
            {
                HttpContext.Session.SetString("ClienteId", usuario.ClienteId.Value.ToString());
            }
            else
            {
                HttpContext.Session.Remove("ClienteId");
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            bool loginJaExiste = _context.Usuarios.Any(u => u.Login == model.Login);
            if (loginJaExiste)
            {
                ViewBag.Erro = "Já existe um usuário com esse login.";
                return View(model);
            }

            bool emailJaExiste = _context.Usuarios.Any(u => u.Email == model.Email);
            if (emailJaExiste)
            {
                ViewBag.Erro = "Já existe um usuário com esse email.";
                return View(model);
            }

            var cliente = new Cliente
            {
                Nome = model.Nome,
                Email = model.Email,
                Telefone = model.Telefone
            };

            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            var usuario = new Usuario
            {
                Nome = model.Nome,
                Login = model.Login,
                Email = model.Email,
                Telefone = model.Telefone,
                Senha = model.Senha,
                Perfil = "Usuario",
                ClienteId = cliente.Id
            };

            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            TempData["Sucesso"] = "Cadastro realizado com sucesso. Faça login para entrar.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}