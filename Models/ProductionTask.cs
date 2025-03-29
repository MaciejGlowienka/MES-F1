using System.Diagnostics.CodeAnalysis;

namespace MES_F1.Models
{
    public class ProductionTask
    {
        public int ProductionTaskId { get; set; }

        public string TaskName { get; set; }

        public int InstructionStep { get; set; }

        [AllowNull]
        public int? TeamId { get; set; }

        public Team? Team { get; set; }
        [AllowNull]
        public int? MachineId { get; set; }

        public Machine? Machine { get; set; }

        public int ProductionId { get; set; }

        public Production? Production { get; set; }
        [AllowNull]
        public DateTime? PlannedStartTime { get; set; }
        [AllowNull]
        public DateTime? PlannedEndTime { get; set; }
        [AllowNull]
        public DateTime? ActualStartTime { get; set; }
        [AllowNull]
        public DateTime? ActualEndTime { get; set; }

        public ProductionTask() { }

    }
}
