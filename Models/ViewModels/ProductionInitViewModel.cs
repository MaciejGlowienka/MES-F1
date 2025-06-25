namespace MES_F1.Models.ViewModels
{
    public class ProductionInitViewModel
    {
        public List<Instruction> Instructions { get; set; }

        public int? InstructionId { get; set; }

        public ProductionState State { get; set; } = ProductionState.None;
    }
}
