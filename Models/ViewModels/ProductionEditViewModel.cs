using Mono.TextTemplating;

namespace MES_F1.Models.ViewModels
{
    public class ProductionEditViewModel
    {
        public int ProductionId { get; set; }

        public ProductionState State { get; set; }

        public ProductionTask? Task { get; set; }
    }
}
