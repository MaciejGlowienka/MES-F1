using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MES_F1.Models
{
    public class Worker
    {
        public int WorkerId { get; set; }

        public string WorkerName { get; set; }

        
        public string? AccountId { get; set; }

        [ForeignKey(nameof(AccountId))]
        public ApplicationUser? User { get; set; }

        public ICollection<TeamWorkerRoleAssign> TeamWorkerRoleAssignments { get; set; }
    }
}
