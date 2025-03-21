namespace MES_F1.Models
{
    public class Instruction
    {
        public int InstructionId { get; set; }
        public string? InstructionName { get; set; }
        public string? InstructionURL { get; set; }
        public PartType InstructionPartType { get; set; } = 0;

    }
}
