using System.ComponentModel;

namespace MES_F1.Models
{
    public enum PartType
    {
        Other = 0,
        [Description("Front Wing")] FrontWing = 1,
        [Description("Rear Wing")] RearWing = 2,
        [Description("Monocoque")] Monocoque = 3,
        [Description("Wheel Cover")] WheelCover = 4,
        [Description("Floor")] Floor = 5,
        [Description("Engine Cover")] EngineCover = 6,
        [Description("Suspension")] Suspension = 7,
        [Description("Cooling Pipe")] CoolingPipe = 8,
        [Description("Brake")] Brake = 9,
        [Description("Radiator")] Radiator = 10,
    }
}
