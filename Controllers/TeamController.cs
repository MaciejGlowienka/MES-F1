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
        public IActionResult TeamAssign(int? teamId)
        {
            ViewBag.Teams = _context.Teams.ToList();
            ViewBag.Workers = _context.Workers.Where(w => w.TeamId == null).ToList();
            ViewBag.TeamRoles = _context.TeamRoles.ToList();


            if (teamId != null)
            {
                var team = _context.Teams.FirstOrDefault(t => t.TeamId == teamId);
                ViewBag.TeamName = team.TeamName;
                ViewBag.TeamWorkScope = EnumHelper.GetDescription(team.TeamWorkScope);
                ViewBag.TeamId = teamId;
                ViewBag.WorkersWithRoles = GetWorkerWithRoles((int)teamId);
            }

            return View();
        }

        [HttpPost]
        public IActionResult TeamWorkerAssign(TeamAssignViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return RedirectToAction("TeamAssign");
            }

            var worker = _context.Workers.FirstOrDefault(w => w.WorkerId == model.WorkerId);

            if (worker != null)
            {
                worker.TeamId = model.TeamId;
                worker.TeamRoleId = model.TeamRoleId;
                _context.SaveChanges();
            }

            return RedirectToAction("TeamAssign", new { model.TeamId });
        }


        [HttpPost]
        public IActionResult TeamDisplay(int TeamId)
        {
            return RedirectToAction("TeamAssign", new { TeamId });
        }

        [HttpPost]
        public IActionResult RemoveWorkerFromTeam(int TeamId, int WorkerId)
        {
            var worker = _context.Workers.FirstOrDefault(w => w.WorkerId == WorkerId);

            if (worker != null)
            {
                worker.TeamId = null;
                worker.TeamRoleId = null;
                _context.SaveChanges();
            }

            return RedirectToAction("TeamAssign", new { TeamId });
        }

        [HttpPost]
        public IActionResult RemoveTeam(int TeamId)
        {
            var team = _context.Teams.FirstOrDefault(t => t.TeamId == TeamId);
            var workers = _context.Workers.Where(w => w.TeamId == TeamId).ToList();
            if (team == null)
            {
                return NotFound();
            }

            foreach (var worker in workers)
            {
                worker.TeamId = null;
                _context.Workers.Update(worker);
            }

            _context.Teams.Remove(team);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Team has been removed.";
            return RedirectToAction("TeamAssign");
        }


        public object GetWorkerWithRoles(int TeamId)
        {
            return _context.Workers
                            .Where(w => w.TeamId == TeamId)
                            .Select(w => new
                            {
                                WorkerId = w.WorkerId,
                                WorkerName = w.WorkerName,
                                RoleId = w.TeamRoleId,
                                RoleName = w.TeamRole != null ? w.TeamRole.RoleName : "Brak roli"
                            })
                            .ToList();
        }
        
        public IActionResult CreateTeam()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateTeam(string TeamName, WorkScope TeamWorkScope)
        {
            if (TeamName != null || TeamWorkScope != null) 
            {
                var team = new Team(TeamName, TeamWorkScope);
                _context.Teams.Add(team);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Team has been successfully created.";
            }

            return RedirectToAction("TeamAssign");
        }

    }
}
