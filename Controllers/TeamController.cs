using MES_F1.Data;
using MES_F1.Models;
using MES_F1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MES_F1.Controllers
{
    public class TeamController : Controller
    {

        private readonly ILogger<CalendarController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TeamController(ILogger<CalendarController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Route("Team/TeamAssign")]
        public async Task<IActionResult> TeamAssign(int? teamId)
        {
            var viewModel = new TeamAssignPageViewModel
            {
                Teams = await _context.Teams.ToListAsync(),
                AvailableWorkers = await _context.Workers.Where(w => w.TeamId == null).ToListAsync(),
                TeamRoles = await _context.TeamRoles.ToListAsync()
            };

            if (teamId.HasValue)
            {
                var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamId == teamId);
                if (team != null)
                {
                    viewModel.TeamName = team.TeamName;
                    viewModel.TeamWorkScope = EnumHelper.GetDescription(team.TeamWorkScope);
                    viewModel.SelectedTeamId = team.TeamId;
                    viewModel.WorkersWithRoles = await GetWorkerWithRolesAsync(team.TeamId);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> TeamWorkerAssign([Bind(Prefix = "Assignment")] TeamAssignViewModel model)
        {
            if (!ModelState.IsValid)
            {
                // Możesz przekazać TeamId jako route param do powrotu na stronę TeamAssign
                return RedirectToAction("TeamAssign", new { teamId = model.TeamId });
            }

            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.WorkerId == model.WorkerId);
            if (worker != null)
            {
                worker.TeamId = model.TeamId;
                worker.TeamRoleId = model.TeamRoleId;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("TeamAssign", new { teamId = model.TeamId });
        }



        [HttpPost]
        public IActionResult TeamDisplay(int TeamId)
        {
            return RedirectToAction("TeamAssign", new { TeamId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveWorkerFromTeam(int TeamId, int WorkerId)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.WorkerId == WorkerId);
            if (worker != null)
            {
                worker.TeamId = null;
                worker.TeamRoleId = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("TeamAssign", new { TeamId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveTeam(int TeamId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamId == TeamId);
            if (team == null)
            {
                return NotFound();
            }

            var workers = await _context.Workers.Where(w => w.TeamId == TeamId).ToListAsync();
            foreach (var worker in workers)
            {
                worker.TeamId = null;
            }

            var ProductionTasks = await _context.ProductionTasks.Where(w => w.TeamId == TeamId).ToListAsync();
            foreach (var Task in ProductionTasks)
            {
                var Id = TeamId;
                Task.TeamId = Id;
            }


            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Team has been removed.";
            return RedirectToAction("TeamAssign");
        }

        private async Task<List<WorkerWithRoleViewModel>> GetWorkerWithRolesAsync(int teamId)
        {
            return await _context.Workers
                .Where(w => w.TeamId == teamId)
                .Select(w => new WorkerWithRoleViewModel
                {
                    WorkerId = w.WorkerId,
                    WorkerName = w.WorkerName,
                    RoleId = w.TeamRoleId,
                    RoleName = w.TeamRole != null ? w.TeamRole.RoleName : "Brak roli"
                })
                .ToListAsync();
        }

        public IActionResult CreateTeam()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam(string TeamName, WorkScope TeamWorkScope)
        {
            if (!string.IsNullOrWhiteSpace(TeamName))
            {
                var team = new Team(TeamName, TeamWorkScope);
                _context.Teams.Add(team);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Team has been successfully created.";
            }

            return RedirectToAction("TeamAssign");
        }
    }
}