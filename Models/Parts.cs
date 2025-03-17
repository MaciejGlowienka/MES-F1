namespace MES_F1.Models
{
    public class Parts
    {
        public int PartsId {  get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string type { get; set; }

        public int ProductionId { get; set; }

        public Production Production { get; set; }


    }
}
