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
        public async Task<IActionResult> CalendarView(int? teamId)
        {
            var userId = _userManager.GetUserId(User);

            var model = new CalendarViewModel();

            if (User.IsInRole("Admin"))
            {
                model.IsAdmin = true;
                model.Teams = await _context.Teams.ToListAsync();
                model.TeamId = teamId;
            }
            else
            {
                var worker = await _context.Workers.FirstOrDefaultAsync(w => w.AccountId == userId);

                if (worker == null)
                    return Unauthorized();

                var activeAssignment = await _context.WorkerTeamHistories
                    .Where(h => h.WorkerId == worker.WorkerId && h.UnassignedAt == null)
                    .OrderByDescending(h => h.AssignedAt)
                    .FirstOrDefaultAsync();

                model.TeamId = activeAssignment?.TeamId;
            }

            return View(model);
        }


        [Authorize(Roles = "Worker,Director,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetProductionWorkSessions(int productionId)
        {
            var sessions = await _context.WorkSessions
                .Include(ws => ws.ProductionTask)
                .ThenInclude(pt => pt.Team)
                .Where(ws => ws.ProductionTask.ProductionId == productionId)
                .ToListAsync();

            var colors = new[] { "#e74c3c", "#3498db", "#2ecc71", "#f1c40f", "#9b59b6", "#1abc9c" };
            var colorMap = new Dictionary<int, string>();
            int colorIndex = 0;

            var events = sessions.Select(ws =>
            {
                if (!colorMap.ContainsKey(ws.ProductionTaskId))
                {
                    colorMap[ws.ProductionTaskId] = colors[colorIndex % colors.Length];
                    colorIndex++;
                }

                return new
                {
                    title = $"{ws.ProductionTask.TaskName} ({ws.ProductionTask.Team?.TeamName ?? "Team"})",
                    start = ws.StartTime.ToString("s"),
                    end = ws.EndTime?.ToString("s"),
                    color = colorMap[ws.ProductionTaskId]
                };
            });

            return Json(events);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamTasks(int teamId)
        {
            var teamTasks = await _context.ProductionTasks
                .Where(t => t.TeamId == teamId &&
                            t.PlannedStartTime.HasValue &&
                            t.PlannedEndTime.HasValue &&
                            t.PlannedEndTime > DateTime.Now)
                .Select(t => new
                {
                    t.TaskName,
                    PlannedStartTime = t.PlannedStartTime.HasValue ? t.PlannedStartTime.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null,
                    PlannedEndTime = t.PlannedEndTime.HasValue ? t.PlannedEndTime.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null
                })
                .ToListAsync();

            return Json(teamTasks);
        }

        [HttpGet]
        public async Task<IActionResult> GetTeamTasksForCalendar(int teamId)
        {
            var tasks = await _context.ProductionTasks
                .Where(t => t.TeamId == teamId && t.PlannedStartTime.HasValue && t.PlannedEndTime.HasValue)
                .Select(t => new {
                    id = t.ProductionTaskId,
                    title = t.TaskName,
                    start = t.PlannedStartTime.Value.ToString("s"),
                    end = t.PlannedEndTime.Value.ToString("s"),
                    isCompleted = t.ActualEndTime.HasValue
                })
                .ToListAsync();

            return Json(tasks);
        }
    }
}
