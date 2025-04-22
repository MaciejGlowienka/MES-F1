using MES_F1.Data;
using MES_F1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace MES_F1.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ILogger<CalendarController> _logger;
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<ApplicationUser> _userManager;

        public CalendarController(ILogger<CalendarController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        [Route("Calendar/CalendarView")]
        public async Task<IActionResult> CalendarView()
        {
            var userId = _userManager.GetUserId(User);

            var worker = await _context.Workers
                .Include(w => w.Team)
                .FirstOrDefaultAsync(w => w.AccountId == userId);

            if (worker == null)
            {
                return Unauthorized();
            }

            if(worker.TeamId != null)
            {
                var teamId = worker.TeamId.Value;
                ViewBag.TeamId = teamId;
            }


            return View();
        }
    }
}
