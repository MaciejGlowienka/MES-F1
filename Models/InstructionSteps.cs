using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models
{
    public class InstructionSteps
    {
        [Key]
        public int InstructionStepId { get; set; }

        public int InstructionId { get; set; }

        public Instruction? Instruction { get; set; }

        public int InstructionStepNumber { get; set; }

        public WorkScope StepWorkScope { get; set; } = 0;

        public string? InstructionStepDescription { get; set; }

        public int EstimatedDurationMinutes { get; set; } = 60;
    }
}
