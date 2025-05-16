using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace MES_F1.Models
{
    public class Worker
    {
        public int WorkerId { get; set; }

        public string WorkerName { get; set; }

        
        public string? AccountId { get; set; }

        public ApplicationUser? User { get; set; }


    }
}
