using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models.ViewModels
{
    public class TeamAssignViewModel
    {
        [Required]
        public int TeamId { get; set; }
        [Required(ErrorMessage = "Choose worker.")]
        public int WorkerId { get; set; }
        [Required(ErrorMessage = "Choose role.")]
        public int TeamRoleId { get; set; }
    }
}
