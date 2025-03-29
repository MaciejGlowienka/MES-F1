using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models.ViewModels
{
    public class ProductionTaskViewModel
    {
        [Required]
        public int InstructionStep { get; set; }

        [Required]
        public string TaskName { get; set; }

        [Required(ErrorMessage = "Wybierz zespół.")]
        public int? TeamId { get; set; }

        [Required]
        public int ProductionId { get; set; }
    }
}
