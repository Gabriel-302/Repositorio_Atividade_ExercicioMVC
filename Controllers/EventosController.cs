using Microsoft.AspNetCore.Mvc;
using ExercicioMVC.Models;
using ExercicioMVC.ViewModels;

namespace ExercicioMVC.Controllers
{
    public class EventosController : Controller
    {
        private static List<Evento> _eventos = new()
        {
            new Evento
            {
                Id = 1,
                Nome = "Festa de Aniversário",
                Descricao = "Festa de aniversário com música ao vivo e comida deliciosa.",
                Local = "Tatuapé",
                DataEvento = new DateTime(2024, 01, 12),
                QuantidadeVagas = 100,
                ValorIngresso = 50.00m
            },
            new Evento
            {
                Id = 2,
                Nome = "Conferência de Tecnologia",
                Descricao = "Reunião de profissionais de tecnologia para discutir as últimas tendências e inovações.",
                Local = "Limeira",
                DataEvento = new DateTime(2024, 12, 12),
                QuantidadeVagas = 200,
                ValorIngresso = 75.00m
            }
        };

        public IActionResult Index()
        {
            return View(_eventos);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            // Retorna a view para criar um novo evento
            return View();
        }

        [HttpPost]
        public IActionResult Criar(Evento evento)
        {
            if (!ModelState.IsValid)
            {
                return View(evento);
            }

            // Gerar um novo Id para o evento
            evento.Id = _eventos.Any() ? _eventos.Max(e => e.Id) + 1 : 1;
            
            // Adicionar o evento à lista de eventos
            _eventos.Add(evento);

            // Redirecionar para a página de listagem de eventos
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Buscar()
        {
            return View(new BuscaEventoViewModel());
        }

        // POST /Eventos/Buscar
        [HttpPost]
        public IActionResult Buscar(BuscaEventoViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.NomeBusca) && string.IsNullOrWhiteSpace(vm.LocalBusca) && !vm.DataEventoBusca.HasValue)
            {
                return View(vm);
            }

            var query = _eventos.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(vm.NomeBusca))
            {
                var nome = vm.NomeBusca.Trim();

                if (_eventos.Any(a => a.Nome != null && a.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase)))
                {
                    query = query.Where(e => e.Nome != null && e.Nome.Contains(nome, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    ModelState.AddModelError(nameof(vm.NomeBusca), "Não existe nenhum evento com o nome informado.");
                }
            }

            if (!string.IsNullOrWhiteSpace(vm.LocalBusca))
            {
                var local = vm.LocalBusca.Trim();

                if (_eventos.Any(a => a.Local != null && a.Local.Contains(local, StringComparison.OrdinalIgnoreCase)))
                {
                    query = query.Where(e => e.Local != null && e.Local.Contains(local, StringComparison.OrdinalIgnoreCase));
                }
                else
                {
                    ModelState.AddModelError(nameof(vm.LocalBusca), "Não existe nenhum evento no local informado.");
                }
            }

            if (vm.DataEventoBusca.HasValue)
            {
                var data = vm.DataEventoBusca.Value.Date;

                // Se DataEvento na model Evento for DateTime?, use: a.DataEvento?.Date == data
                if (_eventos.Any(a => a.DataEvento.Date == data))
                {
                    query = query.Where(e => e.DataEvento.Date == data);
                }
                else
                {
                    ModelState.AddModelError(nameof(vm.DataEventoBusca), "Não existe nenhum evento na data informada.");
                }
            }

            if (ModelState.IsValid)
            {
                vm.Resultados = query.ToList();

                if (!vm.Resultados.Any())
                {
                    ModelState.AddModelError(string.Empty, "Nenhum evento encontrado combinando todos os filtros informados.");
                }
            }

            return View(vm);
        }

        public IActionResult Detalhes(int id)
        {
            var evento = _eventos.FirstOrDefault(e => e.Id == id);
            if (evento == null)
            {
                return NotFound();
            }

            return View(evento);
        }
    }
}