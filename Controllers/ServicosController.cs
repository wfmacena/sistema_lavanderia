using Microsoft.AspNetCore.Mvc;
using SistemaLavanderia.Models;

namespace SistemaLavanderia.Controllers
{
    public class ServicosController : Controller
    {
        public IActionResult Index()
        {
            var servicos = new List<ServicoViewModel>
            {
                new ServicoViewModel
                {
                    Nome = "Lavagem Simples",
                    Descricao = "Lavagem padrão",
                    PrecoBase = 10.00m,
                    UnidadeMedida = "Peça"
                },
                new ServicoViewModel
                {
                    Nome = "Lavagem a Seco",
                    Descricao = "Lavagem especial",
                    PrecoBase = 25.00m,
                    UnidadeMedida = "Peça"
                },
                new ServicoViewModel
                {
                    Nome = "Passadoria",
                    Descricao = "Passar roupa",
                    PrecoBase = 8.00m,
                    UnidadeMedida = "Peça"
                },
                new ServicoViewModel
                {
                    Nome = "Lavagem por Quilo",
                    Descricao = "Preço por quilo",
                    PrecoBase = 35.00m,
                    UnidadeMedida = "Kg"
                },
                new ServicoViewModel
                {
                    Nome = "Tinturaria",
                    Descricao = "Tingimento e restauração de roupas",
                    PrecoBase = 70.00m,
                    UnidadeMedida = "Peça"
                }
            };

            return View(servicos);
        }
    }
}