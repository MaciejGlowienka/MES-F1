using System.ComponentModel.DataAnnotations.Schema;

namespace MES_F1.Models
{
    public class TeamWorkerRoleAssign
    {
        public int TeamWorkerRoleAssignId { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }
        public int WorkerId { get; set; }
        public Worker Worker { get; set; }
        public int TeamRoleId { get; set; }
        public TeamRole TeamRole { get; set; }

    }
}
