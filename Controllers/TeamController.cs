using MES_F1.Data;
using MES_F1.Models;
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
        public IActionResult TeamAssign(int? TeamId)
        {
            ViewBag.Teams = _context.Teams.ToList();
            ViewBag.Workers = _context.Workers.Where(w => w.TeamId == null).ToList();
            ViewBag.TeamRoles = _context.TeamRoles.ToList();
            ViewBag.TeamId = TeamId;

            if (TeamId != null)
            {
                ViewBag.WorkersWithRoles = GetWorkerWithRoles((int)TeamId);
            }

            return View();
        }

        [HttpPost]
        public IActionResult TeamWorkerAssign(int TeamId, int WorkerId, int TeamRoleId)
        {

            var worker = _context.Workers.FirstOrDefault(w => w.WorkerId == WorkerId);

            if (worker != null)
            {
                worker.TeamId = TeamId;
                worker.TeamRoleId = TeamRoleId;
                _context.SaveChanges();
            }

            return RedirectToAction("TeamAssign", new { TeamId });
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

    }
}
