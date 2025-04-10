using MES_F1.Data;
using MES_F1.Models;
using MES_F1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public IActionResult Index()
        {
            ViewBag.Instructions = _context.Instructions.ToList();
            
            return View();
        }

        [HttpPost]
        public IActionResult ProductionCreate(int InstructionId, ProductionState state)
        {

            var Instruction = _context.Instructions.FirstOrDefault(w => w.InstructionId == InstructionId);

            if (Instruction == null)
            {
                return NotFound("Nie znaleziono instrukcji.");
            }

            Production prod = new Production()
            {
                StartTime = DateTime.Now,
                InstructionId = InstructionId,
                Name = Instruction.InstructionName + " " + DateTime.Now,
                State = state
            };

            _context.Productions.Add(prod);
            _context.SaveChanges();
            _context.Entry(prod).GetDatabaseValues();

            prod.Name = Instruction.InstructionName + " " + prod.ProductionId;

            var steps = _context.InstructionSteps.Where(w => w.InstructionId == InstructionId).ToList();

            foreach (InstructionSteps step in steps)
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
            ViewBag.Productions = null;
            var model = new ProductionListViewModel();
            if (state != null)
            {
                ViewBag.Productions = _context.Productions.Where(w => w.State == state).ToList();
                ViewBag.ProductionState = state;

                model.State = (ProductionState)state;
            }
            
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
                .Where(t => t.TeamId == teamId && t.PlannedStartTime.HasValue && t.PlannedEndTime.HasValue)
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
                    ModelState.AddModelError("", $"Ten task rozpoczyna się przed końcem poprzedniego kroku (Step {previousTask.InstructionStep}) o {previousTask.PlannedEndTime.Value:yyyy-MM-dd HH:mm}.");
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
                    ModelState.AddModelError("", $"Ten task kończy się po rozpoczęciu następnego kroku (Step {nextTask.InstructionStep}) o {nextTask.PlannedStartTime.Value:yyyy-MM-dd HH:mm}.");
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
                    ModelState.AddModelError("", "Wybrany zespół ma już inne zadanie zaplanowane w tym czasie.");

                }
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
