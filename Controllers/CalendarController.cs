using MES_F1.Data;
using MES_F1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MES_F1.Models.ViewModels;


namespace MES_F1.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Worker,Admin")]
        [Route("Calendar/CalendarView")]
        public async Task<IActionResult> CalendarView()
        {
            var userId = _userManager.GetUserId(User);

            var worker = await _context.Workers
                .FirstOrDefaultAsync(w => w.AccountId == userId);

            if (worker == null)
            {
                return Unauthorized();
            }

            var activeAssignment = await _context.WorkerTeamHistories
                .Where(h => h.WorkerId == worker.WorkerId && h.UnassignedAt == null)
                .OrderByDescending(h => h.AssignedAt)
                .FirstOrDefaultAsync();

            var model = new CalendarViewModel
            {
                TeamId = activeAssignment?.TeamId
            };


            return View(model);
        }
    }
}
