namespace MES_F1.Models.ViewModels
{
    public class ProductionSetupViewModel
    {
        public int ProductionId { get; set; }
        public string ProductionName { get; set; }
        public ProductionState State { get; set; }

        public List<ProductionTask> ProductionTasks { get; set; }
    }
}
