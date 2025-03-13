using System.ComponentModel;

namespace MES_F1.Models
{
    public enum WorkScope
    {
        None = 0,

        [Description("Metal Forming")] MetalForming = 1,

        [Description("Engine Assembling")] EngineAssembling = 2,

        [Description("Composite Laying")] CompositeLaying = 3,

        [Description("Lamination")] Lamination = 4,

        [Description("Additive Finishing")] AdditiveFinishing = 5,

        [Description("Painting")] Painting = 6,
    }
}
