using MES_F1.Data;
using MES_F1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult TeamAssign()
        {
            ViewBag.Teams = _context.Teams.ToList();
            ViewBag.Workers = _context.Workers.ToList();
            ViewBag.TeamRoles = _context.TeamRoles.ToList();

            return View();
        }
    }
}
