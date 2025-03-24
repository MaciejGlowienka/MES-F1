using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models.ViewModels
{
    public class ProductionCreateViewModel
    {
        [Required]
        public int InstructionId { get; set; }

        [Required]
        public List<ProductionTaskViewModel> Tasks { get; set; }
    }
}
