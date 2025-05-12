using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models
{
    public class Team
    {

        public int TeamId { get; set; }

        [Required(ErrorMessage = "Enter the team name")]
        public string TeamName { get; set; }

        public WorkScope TeamWorkScope { get; set; } = 0;

        public bool IsArchived { get; set; } = false;

        public Team() { }

        public Team(string name, WorkScope workScope)
        {
            TeamName = name;
            TeamWorkScope = workScope;
        }


    }
}
