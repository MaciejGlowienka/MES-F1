namespace MES_F1.Models.ViewModels
{
    public class CalendarViewModel
    {
        public int? TeamId { get; set; }
        public bool IsAdmin { get; set; }
        public List<Team> Teams { get; set; } = new();
    }
}