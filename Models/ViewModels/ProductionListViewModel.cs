namespace MES_F1.Models.ViewModels
{
    public class ProductionListViewModel
    {
        public ProductionState? State { get; set; }
        public List<Production> Productions { get; set; }
    }
}