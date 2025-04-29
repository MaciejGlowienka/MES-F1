using MES_F1.Migrations;

namespace MES_F1.Models.ViewModels
{
    public class WorkplaceViewModel
    {
        public InstructionSteps InstructionStep { get; set; }
        public ProductionTask ProductionTask { get; set; }
        public List<WorkSession> WorkSessions { get; set; } = new();
        public WorkSession? CurrentSession { get; set; }
    }
}
