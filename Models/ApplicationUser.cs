using Microsoft.AspNetCore.Identity;

namespace MES_F1.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime Birthday { get; set; }

        public ICollection<Worker>? Workers { get; set; }

    }
}
