using MES_F1.Data;
using MES_F1.Models;
using MES_F1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace MES_F1.Controllers
{
    public class ProductionController : Controller
    {

        private readonly ILogger<CalendarController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ProductionController(ILogger<CalendarController> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }

        public IActionResult Index(int? instructionId = null)
        {
            var model = new ProductionIndexViewModel
            {
                Instructions = _context.Instructions.ToList(),
                InstructionId = instructionId
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult ProductionCreate(ProductionIndexViewModel model)
        {

            var instruction = _context.Instructions.FirstOrDefault(w => w.InstructionId == model.InstructionId);

            if (instruction == null)
            {
                return NotFound("Nie znaleziono instrukcji.");
            }

            var prod = new Production()
            {
                StartTime = DateTime.Now,
                InstructionId = instruction.InstructionId,
                Name = instruction.InstructionName + " " + DateTime.Now,
                State = model.State
            };

            _context.Productions.Add(prod);
            _context.SaveChanges();
            _context.Entry(prod).GetDatabaseValues();

            prod.Name = instruction.InstructionName + " " + prod.ProductionId;

            var steps = _context.InstructionSteps
                .Where(w => w.InstructionId == instruction.InstructionId)
                .ToList();

            foreach (var step in steps)
            {
                ProductionTask productionTask = new ProductionTask()
                {
                    ProductionId = prod.ProductionId,
                    InstructionStep = step.InstructionStepNumber,
                    TaskName = step.InstructionStepDescription
                };

                _context.ProductionTasks.Add(productionTask);
            }
            _context.SaveChanges();

            return RedirectToAction("ProductionList", new { prod.State });
        }


        public IActionResult ProductionList(ProductionState? state)
        {
            var model = new ProductionListViewModel
            {
                State = state ?? ProductionState.None,
                Productions = state != null
                    ? _context.Productions.Where(w => w.State == state).ToList()
                    : new List<Production>()
            };

            return View(model);
        }


        [HttpGet]
        public IActionResult ProductionSetup(int productionId)
        {
            var production = _context.Productions.FirstOrDefault(p => p.ProductionId == productionId);
            if (production == null)
            {
                return NotFound();
            }
            var model = new ProductionEditViewModel
            {
                ProductionId = production.ProductionId,
                State = production.State,
                ProductionName = production.Name
            };

            ViewBag.ProductionTasks = _context.ProductionTasks
                .Include(t => t.Team)
                .Include(t => t.Machine)
                .Where(w => w.ProductionId == productionId)
                .ToList();


                return View(model);
        }

        [HttpPost]
        public IActionResult ProductionEdit(int productionId, ProductionState State)
        {
            var prod = _context.Productions.FirstOrDefault(w => w.ProductionId == productionId);
            prod.State = State;
            _context.Productions.Update(prod);
            _context.SaveChanges();

            return RedirectToAction("ProductionList", new { prod.State });
        }

        [HttpGet]
        public IActionResult GetTeamTasks(int teamId)
        {
            var teamTasks = _context.ProductionTasks
                .Where(t => t.TeamId == teamId && t.PlannedStartTime.HasValue && t.PlannedEndTime.HasValue && t.PlannedEndTime > DateTime.Now)
                .Select(t => new
                {
                    t.TaskName,
                    // Sprawdzanie, czy PlannedStartTime jest null przed wywołaniem ToString
                    PlannedStartTime = t.PlannedStartTime.HasValue ? t.PlannedStartTime.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null,
                    // Sprawdzanie, czy PlannedEndTime jest null przed wywołaniem ToString
                    PlannedEndTime = t.PlannedEndTime.HasValue ? t.PlannedEndTime.Value.ToString("yyyy-MM-ddTHH:mm:ss") : null
                })
                .ToList();

            return Json(teamTasks);
        }

        [HttpGet]
        public IActionResult GetTeamTasksForCalendar(int teamId)
        {
            var tasks = _context.ProductionTasks
                .Where(t => t.TeamId == teamId && t.PlannedStartTime.HasValue && t.PlannedEndTime.HasValue)
                .Select(t => new {
                    id = t.ProductionTaskId, // ← potrzebne do przekierowania
                    title = t.TaskName,
                    start = t.PlannedStartTime.Value.ToString("s"),
                    end = t.PlannedEndTime.Value.ToString("s"),
                    isCompleted = t.ActualEndTime.HasValue
                })
                .ToList();

            return Json(tasks);
        }

        [HttpPost]
        public IActionResult TaskEdit(int TaskId)
        {
            var task = _context.ProductionTasks
                .Include(t => t.Production)
                .Include(t => t.Team)
                .Include(t => t.Machine)
                .FirstOrDefault(t => t.ProductionTaskId == TaskId);

            if (task == null)
            {
                return NotFound();
            }

            var instructionStep = _context.InstructionSteps
                .FirstOrDefault(s =>
                    s.InstructionId == task.Production.InstructionId &&
                    s.InstructionStepNumber == task.InstructionStep);

            int duration = instructionStep?.EstimatedDurationMinutes ?? 0;

            var model = new TaskEditViewModel
            {
                TaskId = task.ProductionTaskId,
                TaskName = task.TaskName,
                InstructionStep = task.InstructionStep,
                TeamId = task.TeamId,
                MachineId = task.MachineId,
                PlannedStartTime = task.PlannedStartTime,
                PlannedEndTime = task.PlannedEndTime,
                Teams = _context.Teams.ToList(),
                Machines = _context.Machines.ToList(),
                PreviousTask = _context.ProductionTasks.FirstOrDefault(t =>
                    t.ProductionId == task.ProductionId && t.InstructionStep == task.InstructionStep - 1),
                NextTask = _context.ProductionTasks.FirstOrDefault(t =>
                    t.ProductionId == task.ProductionId && t.InstructionStep == task.InstructionStep + 1),
                EstimatedDurationMinutes = duration
            };

            return View("TaskEdit", model);
        }

        [HttpPost]
        public IActionResult TaskSave(TaskEditViewModel model)
        {
            var task = _context.ProductionTasks.FirstOrDefault(t => t.ProductionTaskId == model.TaskId);
            if (task == null)
            {
                return NotFound();
            }

            // --- Sprawdzenie kolizji z poprzednim taskiem ---
            var previousTask = _context.ProductionTasks.FirstOrDefault(t =>
                t.ProductionId == task.ProductionId &&
                t.InstructionStep == task.InstructionStep - 1);

            if (previousTask != null && previousTask.PlannedEndTime.HasValue && model.PlannedStartTime.HasValue)
            {
                if (model.PlannedStartTime < previousTask.PlannedEndTime)
                {
                    ModelState.AddModelError("", $"This task starts before the previous step ends (Step {previousTask.InstructionStep}) - {previousTask.PlannedEndTime.Value:yyyy-MM-dd HH:mm}.");
                }
            }

            // --- Sprawdzenie kolizji z następnym taskiem ---
            var nextTask = _context.ProductionTasks.FirstOrDefault(t =>
                t.ProductionId == task.ProductionId &&
                t.InstructionStep == task.InstructionStep + 1);

            if (nextTask != null && nextTask.PlannedStartTime.HasValue && model.PlannedEndTime.HasValue)
            {
                if (model.PlannedEndTime > nextTask.PlannedStartTime)
                {
                    ModelState.AddModelError("", $"This task ends after the next step starts (Step {nextTask.InstructionStep}) - {nextTask.PlannedStartTime.Value:yyyy-MM-dd HH:mm}.");
                }
            }

            // --- Sprawdzenie kolizji z innymi taskami tego samego zespołu ---
            if (model.TeamId.HasValue && model.PlannedStartTime.HasValue && model.PlannedEndTime.HasValue)
            {
                var overlappingTasks = _context.ProductionTasks
                    .Where(t =>
                        t.ProductionTaskId != model.TaskId &&
                        t.TeamId == model.TeamId &&
                        t.PlannedStartTime.HasValue &&
                        t.PlannedEndTime.HasValue &&
                        (
                            (model.PlannedStartTime < t.PlannedEndTime && model.PlannedStartTime >= t.PlannedStartTime) || // start w środku
                            (model.PlannedEndTime > t.PlannedStartTime && model.PlannedEndTime <= t.PlannedEndTime) ||     // koniec w środku
                            (model.PlannedStartTime <= t.PlannedStartTime && model.PlannedEndTime >= t.PlannedEndTime)    // obejmuje inny
                        )
                    ).ToList();

                if (overlappingTasks.Any())
                {
                    ModelState.AddModelError("", "The selected team already has another task scheduled at this time.");

                }
            }

            if (model.PlannedStartTime < DateTime.Now)
            {
                ModelState.AddModelError("", "Cannot  schedule tasks for a time that has already passed");
            }

            // --- Jeśli są błędy, wróć do widoku z komunikatami ---
            if (!ModelState.IsValid)
            {
                model.Teams = _context.Teams.ToList();
                model.Machines = _context.Machines.ToList();


                return View("TaskEdit", model);
            }

            // --- Zapis taska ---
            task.TeamId = model.TeamId;
            task.MachineId = model.MachineId;
            task.PlannedStartTime = model.PlannedStartTime;
            task.PlannedEndTime = model.PlannedEndTime;

            _context.ProductionTasks.Update(task);
            _context.SaveChanges();

            return RedirectToAction("ProductionSetup", new { productionId = task.ProductionId });
        }


        [HttpPost]
        public IActionResult TaskEditSubmit(TaskEditViewModel model)
        {
            var task = _context.ProductionTasks.FirstOrDefault(t => t.ProductionTaskId == model.TaskId);

            if (task == null)
                return NotFound();

            task.TeamId = model.TeamId;
            task.MachineId = model.MachineId;
            task.PlannedStartTime = model.PlannedStartTime;
            task.PlannedEndTime = model.PlannedEndTime;

            _context.ProductionTasks.Update(task);
            _context.SaveChanges();

            return RedirectToAction("ProductionSetup", new { productionId = task.ProductionId });
        }


        [HttpGet]
        public IActionResult GetProductionWorkSessions(int productionId)
        {
            var sessions = _context.WorkSessions
                .Include(ws => ws.ProductionTask)
                .Include(ws => ws.Team)
                .Where(ws => ws.ProductionTask.ProductionId == productionId)
                .ToList();

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
                    title = $"{ws.ProductionTask.TaskName} ({ws.Team?.TeamName ?? "Team"})",
                    start = ws.StartTime.ToString("s"),
                    end = ws.EndTime?.ToString("s"),
                    color = colorMap[ws.ProductionTaskId]
                };
            });

            return Json(events);
        }


        [HttpGet]
        [Route("Workplace/{taskId}")]
        public IActionResult Workplace(int taskId)
        {
            var task = _context.ProductionTasks
                .Include(t => t.Production)
                .FirstOrDefault(t => t.ProductionTaskId == taskId);

            if (task == null || task.Production == null)
            {
                Console.WriteLine("There was an error with task id:" + taskId);
                return NotFound();
            }

            var step = _context.InstructionSteps.FirstOrDefault(
                i => i.InstructionId == task.Production.InstructionId &&
                     i.InstructionStepNumber == task.InstructionStep);

            if (step == null)
            {
                Console.WriteLine("There was an error with instruction");
                return NotFound();
            }

            var sessions = _context.WorkSessions
                .Where(ws => ws.ProductionTaskId == task.ProductionTaskId)
                .ToList();

            var activeSession = sessions.FirstOrDefault(ws => ws.EndTime == null);

            var model = new WorkplaceViewModel
            {
                ProductionTask = task,
                InstructionStep = step,
                WorkSessions = sessions,
                CurrentSession = activeSession
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult StartTask(int taskId)
        {
            var task = _context.ProductionTasks.FirstOrDefault(t => t.ProductionTaskId == taskId);
            if (task == null || task.TeamId == null)
            {
                return NotFound();
            }
                
            if(task.ActualStartTime == null)
            {
                task.ActualStartTime = DateTime.Now;
            }
                
            var newSession = new WorkSession
            {
                ProductionTaskId = taskId,
                TeamId = task.TeamId.Value,
                StartTime = DateTime.Now
            };

            _context.WorkSessions.Add(newSession);
            _context.SaveChanges();

            return RedirectToAction("Workplace", new { TaskId = taskId });
        }

        [HttpPost]
        public IActionResult StopTask(int sessionId)
        {
            var session = _context.WorkSessions.FirstOrDefault(ws => ws.WorkSessionId == sessionId);
            if (session == null)
                return NotFound();

            session.EndTime = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("Workplace", new { TaskId = session.ProductionTaskId });
        }

        [HttpPost]
        public IActionResult CompleteTask(int taskId)
        {
            var task = _context.ProductionTasks.FirstOrDefault(t => t.ProductionTaskId == taskId);
            if (task == null)
                return NotFound();

            task.ActualEndTime = DateTime.Now;
            _context.SaveChanges();

            return RedirectToAction("Workplace", new { TaskId = taskId });
        }

        [HttpPost]
        public IActionResult RemoveProduction(int ProductionId)
        {
            var prod = _context.Productions.FirstOrDefault(w => w.ProductionId == ProductionId);
            var tasks = _context.ProductionTasks.Where(w => w.ProductionId == ProductionId).ToList();
            if (prod == null)
            {
                return NotFound();
            }

            foreach (var task in tasks)
            {
                _context.ProductionTasks.Remove(task);
            }

            _context.Productions.Remove(prod);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Production has been removed.";
            return RedirectToAction("ProductionList", new {prod.State});
        }
    }
}
