namespace MES_F1.Models
{
    public class MaterialLocation
    {
        public int MaterialLocationId { get; set; }
        public int MaterialId { get; set; }
        public Materials Material { get; set; }
        public string MaterialLocationMessage { get; set; }
        public int? WarehouseSpotId { get; set; }
        public WharehouseSpot WarehouseSpot { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
