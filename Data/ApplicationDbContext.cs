using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MES_F1.Models;
using System.Reflection.Emit;
using Humanizer;

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
        public DbSet<Production> Productions { get; set; }
        public DbSet<ProductionTask> ProductionTasks { get; set; }
        public DbSet<Instruction> Instructions { get; set; }
        public DbSet<InstructionSteps> InstructionSteps { get; set; }
        public DbSet<Machine> Machines { get; set; }


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

            builder.Entity<Machine>()
               .Property(m => m.Status)
               .HasConversion<string>() 
               .HasMaxLength(15)
               .IsRequired();

            builder.Entity<InstructionSteps>()
                .Property(w => w.StepWorkScope)
                .HasConversion<string>()
                .HasMaxLength(25)
                .IsRequired();

            builder.Entity<Instruction>()
                .Property(w => w.InstructionPartType)
                .HasConversion<string>()
                .HasMaxLength(25)
                .IsRequired();

            builder.Entity<Team>()
                .Property(w => w.TeamWorkScope)
                .HasConversion<string>()
                .HasMaxLength(25)
                .IsRequired();

            builder.Entity<Production>()
                .Property(w => w.State)
                .HasConversion<string>()
                .HasMaxLength(25)
                .IsRequired();

            builder.Entity<Production>()
                .HasOne(w => w.Instruction)
                .WithMany()
                .HasForeignKey(w => w.InstructionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductionTask>()
                .HasOne(w => w.Production)
                .WithMany()
                .HasForeignKey(w => w.ProductionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductionTask>()
                .HasOne(w => w.Machine)
                .WithMany()
                .HasForeignKey(w => w.MachineId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductionTask>()
                .HasOne(w => w.Team)
                .WithMany()
                .HasForeignKey(w => w.TeamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<InstructionSteps>()
                .HasOne(w => w.Instruction)
                .WithMany()
                .HasForeignKey(w => w.InstructionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Parts>()
                .HasOne(w => w.Production)
                .WithMany()
                .HasForeignKey(w => w.ProductionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MaterialLocation>()
                .HasOne(w => w.Material)
                .WithMany()
                .HasForeignKey(w => w.MaterialId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MaterialLocation>()
                .HasOne(w => w.WarehouseSpot)
                .WithMany()
                .HasForeignKey(w => w.WarehouseSpotId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PartLocation>()
                .HasOne(w => w.Part)
                .WithMany()
                .HasForeignKey(w => w.PartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PartLocation>()
                .HasOne(w => w.WarehouseSpot)
                .WithMany()
                .HasForeignKey(w => w.WarehouseSpotId)
                .OnDelete(DeleteBehavior.Cascade);

            


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

            builder.Entity<Instruction>().HasData(
                new Instruction { InstructionId = 1, InstructionName = "Front Wing 2025 mid v.1.0", InstructionPartType = PartType.FrontWing },
                new Instruction { InstructionId = 2, InstructionName = "Front Wing 2025 low v.1.0", InstructionPartType = PartType.FrontWing },
                new Instruction { InstructionId = 3, InstructionName = "Front Wing 2025 high v.1.0", InstructionPartType = PartType.FrontWing },
                new Instruction { InstructionId = 4, InstructionName = "Front Wing 2025 mid v.1.1", InstructionPartType = PartType.FrontWing },
                new Instruction { InstructionId = 5, InstructionName = "Right Sidepod Cooling pipe 2025 v.1.0", InstructionPartType = PartType.CoolingPipe },
                new Instruction { InstructionId = 6, InstructionName = "MGU-K Radiator 2025 v.1.0", InstructionPartType = PartType.Radiator }
                );

            builder.Entity<InstructionSteps>().HasData(
                new InstructionSteps { InstructionStepId = 1, InstructionId = 1, InstructionStepNumber = 1, InstructionStepDescription = "Laying layers of carbon fiber", StepWorkScope = WorkScope.CompositeLaying },
                new InstructionSteps { InstructionStepId = 2, InstructionId = 1, InstructionStepNumber = 2, InstructionStepDescription = "Hardening carbon fiber inside an autoclave", StepWorkScope = WorkScope.OperatingMachines },
                new InstructionSteps { InstructionStepId = 3, InstructionId = 1, InstructionStepNumber = 3, InstructionStepDescription = "Paintjob", StepWorkScope = WorkScope.Painting },
                new InstructionSteps { InstructionStepId = 4, InstructionId = 2, InstructionStepNumber = 1, InstructionStepDescription = "Laying layers of carbon fiber", StepWorkScope = WorkScope.CompositeLaying },
                new InstructionSteps { InstructionStepId = 5, InstructionId = 2, InstructionStepNumber = 2, InstructionStepDescription = "Hardening carbon fiber inside an autoclave", StepWorkScope = WorkScope.OperatingMachines },
                new InstructionSteps { InstructionStepId = 6, InstructionId = 2, InstructionStepNumber = 3, InstructionStepDescription = "Paintjob", StepWorkScope = WorkScope.Painting },
                new InstructionSteps { InstructionStepId = 7, InstructionId = 3, InstructionStepNumber = 1, InstructionStepDescription = "Laying layers of carbon fiber", StepWorkScope = WorkScope.CompositeLaying },
                new InstructionSteps { InstructionStepId = 8, InstructionId = 3, InstructionStepNumber = 2, InstructionStepDescription = "Hardening carbon fiber inside an autoclave", StepWorkScope = WorkScope.OperatingMachines },
                new InstructionSteps { InstructionStepId = 9, InstructionId = 3, InstructionStepNumber = 3, InstructionStepDescription = "Paintjob", StepWorkScope = WorkScope.Painting },
                new InstructionSteps { InstructionStepId = 10, InstructionId = 4, InstructionStepNumber = 1, InstructionStepDescription = "Laying layers of carbon fiber", StepWorkScope = WorkScope.CompositeLaying },
                new InstructionSteps { InstructionStepId = 11, InstructionId = 4, InstructionStepNumber = 2, InstructionStepDescription = "Hardening carbon fiber inside an autoclave", StepWorkScope = WorkScope.OperatingMachines },
                new InstructionSteps { InstructionStepId = 12, InstructionId = 4, InstructionStepNumber = 3, InstructionStepDescription = "Paintjob", StepWorkScope = WorkScope.Painting },
                new InstructionSteps { InstructionStepId = 13, InstructionId = 5, InstructionStepNumber = 1, InstructionStepDescription = "Cutting set of metal parts", StepWorkScope = WorkScope.MetalForming },
                new InstructionSteps { InstructionStepId = 14, InstructionId = 5, InstructionStepNumber = 2, InstructionStepDescription = "Welding pipes", StepWorkScope = WorkScope.Welding },
                new InstructionSteps { InstructionStepId = 15, InstructionId = 5, InstructionStepNumber = 3, InstructionStepDescription = "Grinding and blending a weld", StepWorkScope = WorkScope.Welding },
                new InstructionSteps { InstructionStepId = 16, InstructionId = 6, InstructionStepNumber = 1, InstructionStepDescription = "Cutting set of metal parts", StepWorkScope = WorkScope.MetalForming },
                new InstructionSteps { InstructionStepId = 17, InstructionId = 6, InstructionStepNumber = 2, InstructionStepDescription = "Welding radiator plates", StepWorkScope = WorkScope.Welding },
                new InstructionSteps { InstructionStepId = 18, InstructionId = 6, InstructionStepNumber = 3, InstructionStepDescription = "Grinding and blending a weld", StepWorkScope = WorkScope.Welding }
                );

        }
    }
}

