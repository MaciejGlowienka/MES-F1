namespace MES_F1.Models
{
    public class WorkerTeamHistory
    {
        public int WorkerTeamHistoryId { get; set; }

        public int WorkerId { get; set; }
        public Worker Worker { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public int? TeamRoleId { get; set; }
        public TeamRole? TeamRole { get; set; }

        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UnassignedAt { get; set; }
    }
}
