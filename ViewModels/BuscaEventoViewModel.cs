using ExercicioMVC.Models;

namespace ExercicioMVC.ViewModels
{
    public class BuscaEventoViewModel
    {
        public string? NomeBusca { get; set; }
        public string? LocalBusca { get; set; }
        public DateTime? DataEventoBusca { get; set; }
        public List<Evento> Resultados { get; set; } = new();
    }
}