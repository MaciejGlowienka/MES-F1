using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MES_F1.Models;
using System.Reflection.Emit;

namespace MES_F1.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<TeamRole> TeamRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Worker>()
                .HasOne(w => w.User)
                .WithOne()
                .HasForeignKey<Worker>(w => w.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Worker>()
                .HasOne(w => w.Team)
                .WithMany()
                .HasForeignKey(w => w.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Worker>()
                .HasOne(w => w.TeamRole)
                .WithMany()
                .HasForeignKey(w => w.TeamRoleId)
                .OnDelete(DeleteBehavior.Cascade);

            

            builder.Entity<Team>().HasData(
            new Team { TeamId = 1, TeamName = "Development Team" },
            new Team { TeamId = 2, TeamName = "Marketing Team" }
);

            builder.Entity<Worker>().HasData(
                new Worker { WorkerId = 1, WorkerName = "John Doe" },
                new Worker { WorkerId = 2, WorkerName = "Jane Smith" },
                new Worker { WorkerId = 3, WorkerName = "Alice Johnson" }
            );

            builder.Entity<TeamRole>().HasData(
                new TeamRole { TeamRoleId = 1, RoleName = "Developer", RoleDescription = "Developer" },
                new TeamRole { TeamRoleId = 2, RoleName = "Manager", RoleDescription = "Manager" },
                new TeamRole { TeamRoleId = 3, RoleName = "Marketing Specialist", RoleDescription = "Marketing Specialist" }
            );
        }
    }
}

