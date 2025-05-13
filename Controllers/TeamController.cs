using MES_F1.Data;
using MES_F1.Models;
using MES_F1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MES_F1.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Director,Admin")]
        [Route("Team/TeamAssign")]
        public async Task<IActionResult> TeamAssign(int? teamId)
        {
            var viewModel = new TeamAssignPageViewModel
            {
                Teams = await _context.Teams.Where(t => !t.IsArchived).ToListAsync(),
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

        [Authorize(Roles = "Director,Admin")]
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
                var previousHistory = await _context.WorkerTeamHistories
                    .Where(h => h.WorkerId == worker.WorkerId && h.UnassignedAt == null)
                    .FirstOrDefaultAsync();

                if (previousHistory != null)
                {
                    previousHistory.UnassignedAt = DateTime.UtcNow;
                }

                // Aktualne przypisanie
                worker.TeamId = model.TeamId;
                worker.TeamRoleId = model.TeamRoleId;

                // Nowy wpis do historii
                _context.WorkerTeamHistories.Add(new WorkerTeamHistory
                {
                    WorkerId = worker.WorkerId,
                    TeamId = model.TeamId,
                    TeamRoleId = model.TeamRoleId,
                    AssignedAt = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();
            }

            return RedirectToAction("TeamAssign", new { teamId = model.TeamId });
        }


        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public IActionResult TeamDisplay(int TeamId)
        {
            return RedirectToAction("TeamAssign", new { TeamId });
        }

        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveWorkerFromTeam(int TeamId, int WorkerId)
        {
            var worker = await _context.Workers.FirstOrDefaultAsync(w => w.WorkerId == WorkerId);
            if (worker != null)
            {
                var history = await _context.WorkerTeamHistories
                    .Where(h => h.WorkerId == worker.WorkerId && h.UnassignedAt == null)
                    .FirstOrDefaultAsync();

                if (history != null)
                {
                    history.UnassignedAt = DateTime.UtcNow;
                }
                worker.TeamId = null;
                worker.TeamRoleId = null;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("TeamAssign", new { TeamId });
        }

        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveTeam(int TeamId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.TeamId == TeamId);
            if (team == null) return NotFound();

            team.IsArchived = true;

            var workers = await _context.Workers.Where(w => w.TeamId == team.TeamId).ToListAsync();

            foreach (var worker in workers)
            {
                var history = await _context.WorkerTeamHistories
                    .Where(h => h.WorkerId == worker.WorkerId && h.UnassignedAt == null)
                    .FirstOrDefaultAsync();

                if (history != null)
                {
                    history.UnassignedAt = DateTime.UtcNow;
                }

                worker.TeamId = null;
                worker.TeamRoleId = null;
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Team has been archived.";
            return RedirectToAction("TeamAssign");
        }

        [Authorize(Roles = "Director,Admin")]
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

        [Authorize(Roles = "Director,Admin")]
        public IActionResult CreateTeam()
        {
            return View();
        }

        [Authorize(Roles = "Director,Admin")]
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