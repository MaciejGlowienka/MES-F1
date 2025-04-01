using Mono.TextTemplating;
using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models
{
    public class Production
    {
        
        public int ProductionId { get; set; }

        public string Name { get; set; }

        public int? InstructionId { get; set; }

        public Instruction Instruction { get; set; }

        public ProductionState State { get; set; } = ProductionState.None;

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

    }
}
