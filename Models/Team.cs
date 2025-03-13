namespace MES_F1.Models
{
    public class Team
    {

        public int TeamId { get; set; }

        public string TeamName { get; set; }

        public WorkScope TeamWorkScope { get; set; } = 0;


        public Team() { }

        public Team(string name, WorkScope workScope)
        {
            TeamName = name;
            TeamWorkScope = workScope;
        }


    }
}
