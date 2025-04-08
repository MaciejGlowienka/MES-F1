namespace MES_F1.Models.ViewModels
{
    public class TaskEditViewModel
    {
        public int TaskId { get; set; }

        public string TaskName { get; set; }

        public int InstructionStep { get; set; }

        public int EstimatedDurationMinutes { get; set; }

        public int? TeamId { get; set; }
        public int? MachineId { get; set; }

        public DateTime? PlannedStartTime { get; set; }
        public DateTime? PlannedEndTime { get; set; }

        public List<Team> Teams { get; set; } = new();
        public List<Machine> Machines { get; set; } = new();
    }
}