using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models
{
    public class WorkSession
    {
        [Key]
        public int WorkSessionId { get; set; }

        public int ProductionTaskId { get; set; }
        public ProductionTask ProductionTask { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}