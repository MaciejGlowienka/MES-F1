namespace MES_F1.Models.ViewModels
{
    public class TeamAssignPageViewModel
    {
        public List<Team> Teams { get; set; }
        public List<Worker> AvailableWorkers { get; set; }
        public List<TeamRole> TeamRoles { get; set; }

        public string? TeamName { get; set; }
        public string? TeamWorkScope { get; set; }
        public int? SelectedTeamId { get; set; }
        public List<WorkerWithRoleViewModel> WorkersWithRoles { get; set; } = new();

        public TeamAssignViewModel Assignment { get; set; } = new TeamAssignViewModel();
    }
}
