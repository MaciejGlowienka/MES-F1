using System.ComponentModel;

namespace MES_F1.Models
{
    public enum ProductionState
    {
        None = 0,

        [Description("During progress")] DuringProgress = 1,

        [Description("Finished")] Finished = 2,

        [Description("Paused")] Paused = 3,

        [Description("Abandoned")] Abandoned = 4,

    }
}
