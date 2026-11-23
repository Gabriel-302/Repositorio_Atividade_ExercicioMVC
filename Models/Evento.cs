using System.ComponentModel.DataAnnotations;

namespace ExercicioMVC.Models
{
    public class Evento
    {
        public int Id { get; set; }
        
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Nome deve ter entre 2 e 50 caracteres.")]
        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [MinLength(10, ErrorMessage = "A descrição deve ter no mínimo 10 caracteres.")]
        public string Descricao { get; set; } = string.Empty;

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Local deve ter entre 3 e 100 caracteres.")]
        [Required(ErrorMessage = "O local é obrigatório")]
        public string Local { get; set; } = string.Empty;

        [Required(ErrorMessage = "A Data do Evento é obrigatório")]
        [CustomValidation(typeof(Evento), nameof(ValidarDataFutura))]
        public DateTime DataEvento { get; set; }

        [Range(1, 1000, ErrorMessage = "A quantidade de vagas deve ser entre 1 e 1000")]
        [Required(ErrorMessage = "A quantidade de vagas é obrigatória")]
        public int QuantidadeVagas { get; set; }

        [Required(ErrorMessage = "O valor do ingresso é obrigatório")]
        public decimal ValorIngresso { get; set; }

        public static ValidationResult? ValidarDataFutura(DateTime? data)
        {
            if (data.HasValue && data.Value <= DateTime.Now)
            {
                return new ValidationResult("A data do evento deve estar no futuro.");
            }

            return ValidationResult.Success;
        }
    }
}