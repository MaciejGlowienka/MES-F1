namespace MES_F1.Models
{
    public class PartLocation
    {
        public int PartLocationId { get; set; }
        public int PartId { get; set; }
        public Parts Part { get; set; }
        public string PartLocationMessage { get; set; }
        public int? WarehouseSpotId { get; set; }
        public WharehouseSpot WarehouseSpot { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
