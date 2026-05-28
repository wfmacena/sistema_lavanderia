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

        private string SomenteNumeros(string valor)
        {
            return new string(valor.Where(char.IsDigit).ToArray());
        }

        private bool CpfValido(string cpf)
        {
            cpf = SomenteNumeros(cpf);

            if (cpf.Length != 11)
                return false;

            if (new string(cpf[0], cpf.Length) == cpf)
                return false;

            return true;
        }

        [HttpGet]
        public IActionResult Configuracoes()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (!int.TryParse(usuarioIdStr, out int usuarioId))
                return RedirectToAction("Login", "Account");

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == usuarioId);
            if (usuario == null)
                return RedirectToAction("Login", "Account");

            var model = new ConfiguracoesContaViewModel
            {
                UsuarioId = usuario.Id,
                Nome = usuario.Nome,
                Cpf = usuario.Cpf,
                Email = usuario.Email ?? string.Empty,
                Telefone = usuario.Telefone ?? string.Empty
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Configuracoes(ConfiguracoesContaViewModel model)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("UsuarioLogin")))
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            model.Cpf = SomenteNumeros(model.Cpf);
            model.Telefone = SomenteNumeros(model.Telefone);

            if (!CpfValido(model.Cpf))
            {
                ViewBag.Erro = "CPF inválido.";
                return View(model);
            }

            var usuario = _context.Usuarios.FirstOrDefault(u => u.Id == model.UsuarioId);
            if (usuario == null)
                return RedirectToAction("Login", "Account");

            bool emailJaExiste = _context.Usuarios.Any(u => u.Email == model.Email && u.Id != model.UsuarioId);
            if (emailJaExiste)
            {
                ViewBag.Erro = "Já existe outro usuário com esse email.";
                return View(model);
            }

            bool cpfJaExiste = _context.Usuarios.Any(u => u.Cpf == model.Cpf && u.Id != model.UsuarioId);
            if (cpfJaExiste)
            {
                ViewBag.Erro = "Já existe outro usuário com esse CPF.";
                return View(model);
            }

            usuario.Nome = model.Nome;
            usuario.Cpf = model.Cpf;
            usuario.Email = model.Email;
            usuario.Telefone = model.Telefone;

            _context.Usuarios.Update(usuario);

            if (usuario.ClienteId.HasValue)
            {
                var cliente = _context.Clientes.FirstOrDefault(c => c.Id == usuario.ClienteId.Value);
                if (cliente != null)
                {
                    cliente.Nome = model.Nome;
                    cliente.Cpf = model.Cpf;
                    cliente.Email = model.Email;
                    cliente.Telefone = model.Telefone;

                    _context.Clientes.Update(cliente);
                }
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("UsuarioNome", usuario.Nome);

            TempData["Sucesso"] = "Dados atualizados com sucesso.";
            return RedirectToAction("Configuracoes");
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
            {
                ViewBag.Erro = "Preencha todos os campos corretamente.";
                return View(model);
            }

            try
            {
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
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro técnico: {ex.Message} {(ex.InnerException != null ? " | Inner: " + ex.InnerException.Message : "")}";
                return View(model);
            }
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
            {
                ViewBag.Erro = "Por favor, corrija os erros no formulário.";
                return View(model);
            }

            try
            {
                model.Cpf = SomenteNumeros(model.Cpf ?? "");
                model.Telefone = SomenteNumeros(model.Telefone ?? "");

                if (!CpfValido(model.Cpf))
                {
                    ViewBag.Erro = "CPF inválido.";
                    return View(model);
                }

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

                bool cpfJaExiste = _context.Usuarios.Any(u => u.Cpf == model.Cpf);
                if (cpfJaExiste)
                {
                    ViewBag.Erro = "Já existe um usuário com esse CPF.";
                    return View(model);
                }

                var cliente = new Cliente
                {
                    Nome = model.Nome,
                    Cpf = model.Cpf,
                    Email = model.Email,
                    Telefone = model.Telefone
                };

                _context.Clientes.Add(cliente);
                _context.SaveChanges();

                var usuario = new Usuario
                {
                    Nome = model.Nome,
                    Login = model.Login,
                    Cpf = model.Cpf,
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
            catch (Exception ex)
            {
                ViewBag.Erro = "Erro ao criar conta: " + ex.Message;
                return View(model);
            }
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