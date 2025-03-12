namespace MES_F1.Models
{
    public class Machine
    {
        public int MachineId { get; set; }

        public string MachineName { get; set; }

        public string MachineType { get; set; }

        public MachineStatus Status { get; set; } = MachineStatus.Idle;

        public string Localization { get; set; }
    }
}
