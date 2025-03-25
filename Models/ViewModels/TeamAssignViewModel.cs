using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models.ViewModels
{
    public class TeamAssignViewModel
    {
        [Required]
        public int TeamId { get; set; }
        [Required(ErrorMessage = "Wybierz pracownika.")]
        public int WorkerId { get; set; }
        [Required(ErrorMessage = "Wybierz rolę.")]
        public int TeamRoleId { get; set; }
    }
}
