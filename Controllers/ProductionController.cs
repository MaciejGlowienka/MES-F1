﻿using MES_F1.Data;
using MES_F1.Models;
using MES_F1.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System.Threading.Tasks;

namespace MES_F1.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "Director,Admin")]
        public IActionResult ProductionInit(int? instructionId = null)
        {
            var model = new ProductionInitViewModel
            {
                Instructions = _context.Instructions.ToList(),
                InstructionId = instructionId
            };
            return View(model);
        }

        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public async Task<IActionResult> ProductionCreate(ProductionInitViewModel model)
        {

            var instruction = await _context.Instructions.FirstOrDefaultAsync(w => w.InstructionId == model.InstructionId);

            if (instruction == null)
            {
                return NotFound("Instruction not found.");
            }

            var prod = new Production()
            {
                StartTime = DateTime.Now,
                InstructionId = instruction.InstructionId,
                Name = instruction.InstructionName + " " + DateTime.Now,
                State = model.State
            };

            await _context.Productions.AddAsync(prod);
            await _context.SaveChangesAsync();
            await _context.Entry(prod).GetDatabaseValuesAsync();

            prod.Name = instruction.InstructionName + " " + prod.ProductionId;

            var steps = await _context.InstructionSteps
                .Where(w => w.InstructionId == instruction.InstructionId)
                .ToListAsync();

            foreach (var step in steps)
            {
                ProductionTask productionTask = new ProductionTask()
                {
                    ProductionId = prod.ProductionId,
                    InstructionStep = step.InstructionStepNumber,
                    TaskName = step.InstructionStepDescription
                };

                await _context.ProductionTasks.AddAsync(productionTask);
            }
            await _context.SaveChangesAsync();

            return RedirectToAction("ProductionList", new { prod.State });
        }

        [Authorize(Roles = "Director,Admin")]
        public IActionResult ProductionList(ProductionState state = 0)
        {

            var model = new ProductionListViewModel
            {
                State = state,
                Productions = _context.Productions.Where(w => w.State == state).ToList()

            };

            return View(model);
        }

        [Authorize(Roles = "Director,Admin")]
        [HttpGet]
        public async Task<IActionResult> ProductionSetup(int productionId)
        {
            var production = await _context.Productions.FirstOrDefaultAsync(p => p.ProductionId == productionId);
            if (production == null)
            {
                return NotFound();
            }

            var productionTasks = await _context.ProductionTasks
                .Include(t => t.Team)
                .Include(t => t.Machine)
                .Where(w => w.ProductionId == productionId)
                .ToListAsync();

            var model = new ProductionSetupViewModel
            {
                ProductionId = production.ProductionId,
                State = production.State,
                ProductionName = production.Name,
                ProductionTasks = productionTasks
            };

            return View(model);
        }

        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public async Task<IActionResult> ProductionEdit(int productionId, ProductionState State)
        {
            var prod = await _context.Productions.FirstOrDefaultAsync(w => w.ProductionId == productionId);
            if (prod == null)
            {
                return NotFound();
            }

            prod.State = State;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Production state has been changed.";
            return RedirectToAction("ProductionList", new { prod.State });
        }



        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public async Task<IActionResult> TaskEdit(int TaskId)
        {
            var task = await _context.ProductionTasks
                .Include(t => t.Production)
                .Include(t => t.Team)
                .Include(t => t.Machine)
                .FirstOrDefaultAsync(t => t.ProductionTaskId == TaskId);

            if (task == null)
            {
                return NotFound();
            }

            var instructionStep = await _context.InstructionSteps
                .FirstOrDefaultAsync(s =>
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
                Teams = await _context.Teams.ToListAsync(),
                Machines = await _context.Machines.ToListAsync(),
                PreviousTask = await _context.ProductionTasks.FirstOrDefaultAsync(t =>
                    t.ProductionId == task.ProductionId && t.InstructionStep == task.InstructionStep - 1),
                NextTask = await _context.ProductionTasks.FirstOrDefaultAsync(t =>
                    t.ProductionId == task.ProductionId && t.InstructionStep == task.InstructionStep + 1),
                EstimatedDurationMinutes = duration
            };

            return View("TaskEdit", model);
        }

        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public async Task<IActionResult> TaskSave(TaskEditViewModel model)
        {
            var task = await _context.ProductionTasks.FirstOrDefaultAsync(t => t.ProductionTaskId == model.TaskId);
            if (task == null)
            {
                return NotFound();
            }

            
            var previousTask = await _context.ProductionTasks.FirstOrDefaultAsync(t =>
                t.ProductionId == task.ProductionId &&
                t.InstructionStep == task.InstructionStep - 1);

            if (previousTask != null && previousTask.PlannedEndTime.HasValue && model.PlannedStartTime.HasValue)
            {
                if (model.PlannedStartTime < previousTask.PlannedEndTime)
                {
                    ModelState.AddModelError("", $"This task starts before the previous step ends (Step {previousTask.InstructionStep}) - {previousTask.PlannedEndTime.Value:yyyy-MM-dd HH:mm}.");
                }
            }

            
            var nextTask = await _context.ProductionTasks.FirstOrDefaultAsync(t =>
                t.ProductionId == task.ProductionId &&
                t.InstructionStep == task.InstructionStep + 1);

            if (nextTask != null && nextTask.PlannedStartTime.HasValue && model.PlannedEndTime.HasValue)
            {
                if (model.PlannedEndTime > nextTask.PlannedStartTime)
                {
                    ModelState.AddModelError("", $"This task ends after the next step starts (Step {nextTask.InstructionStep}) - {nextTask.PlannedStartTime.Value:yyyy-MM-dd HH:mm}.");
                }
            }

            
            if (model.TeamId.HasValue && model.PlannedStartTime.HasValue && model.PlannedEndTime.HasValue)
            {
                var overlappingTasks = await _context.ProductionTasks
                    .Where(t =>
                        t.ProductionTaskId != model.TaskId &&
                        t.TeamId == model.TeamId &&
                        t.PlannedStartTime.HasValue &&
                        t.PlannedEndTime.HasValue &&
                        (
                            (model.PlannedStartTime < t.PlannedEndTime && model.PlannedStartTime >= t.PlannedStartTime) || 
                            (model.PlannedEndTime > t.PlannedStartTime && model.PlannedEndTime <= t.PlannedEndTime) ||     
                            (model.PlannedStartTime <= t.PlannedStartTime && model.PlannedEndTime >= t.PlannedEndTime)   
                        )
                    ).ToListAsync();

                if (overlappingTasks.Any())
                {
                    ModelState.AddModelError("", "The selected team already has another task scheduled at this time.");

                }
            }

            if (model.PlannedStartTime < DateTime.Now)
            {
                ModelState.AddModelError("", "Cannot  schedule tasks for a time that has already passed");
            }

            
            if (!ModelState.IsValid)
            {
                model.Teams = await _context.Teams.ToListAsync();
                model.Machines = await _context.Machines.ToListAsync();


                return View("TaskEdit", model);
            }

            
            task.TeamId = model.TeamId;
            task.MachineId = model.MachineId;
            task.PlannedStartTime = model.PlannedStartTime;
            task.PlannedEndTime = model.PlannedEndTime;

            _context.ProductionTasks.Update(task);
            await _context.SaveChangesAsync();

            return RedirectToAction("ProductionSetup", new { productionId = task.ProductionId });
        }



        [Authorize(Roles = "Worker,Director,Admin")]
        private async Task<WorkplaceViewModel?> BuildWorkplaceViewModelAsync(int taskId, bool onlyForView = false)
        {
            var task = await _context.ProductionTasks
                .Include(t => t.Production)
                .FirstOrDefaultAsync(t => t.ProductionTaskId == taskId);

            if (task == null || task.Production == null)
            {
                Console.WriteLine("There was an error with task id:" + taskId);
                return null;
            }

            var step = await _context.InstructionSteps.FirstOrDefaultAsync(
                i => i.InstructionId == task.Production.InstructionId &&
                     i.InstructionStepNumber == task.InstructionStep);

            if (step == null)
            {
                Console.WriteLine("There was an error with instruction");
                return null;
            }

            var sessions = await _context.WorkSessions
                .Where(ws => ws.ProductionTaskId == task.ProductionTaskId)
                .ToListAsync();

            var activeSession = sessions.FirstOrDefault(ws => ws.EndTime == null);

            var workerSessions = await _context.WorkerTeamHistories
                .Include(wth => wth.Worker)
                .Include(wth => wth.TeamRole)
                .Where(wth =>
                    wth.TeamId == task.TeamId &&
                    wth.AssignedAt <= (task.ActualEndTime ?? task.PlannedEndTime ?? DateTime.Now) &&
                    (wth.UnassignedAt == null || wth.UnassignedAt >= (task.ActualStartTime ?? task.PlannedStartTime ?? DateTime.Now)))
                .ToListAsync();

            return new WorkplaceViewModel
            {
                ProductionTask = task,
                InstructionStep = step,
                WorkSessions = sessions,
                CurrentSession = activeSession,
                WorkersDuringTask = workerSessions
                    .Select(wth => new WorkerWithRoleViewModel
                    {
                        WorkerId = wth.Worker.WorkerId,
                        WorkerName = wth.Worker.WorkerName,
                        RoleId = wth.TeamRoleId,
                        RoleName = wth.TeamRole.RoleName
                    })
                    .DistinctBy(w => w.WorkerId)
                    .ToList(),
                OnlyForView = onlyForView
            };
        }

        [Authorize(Roles = "Worker,Director,Admin")]
        [HttpGet]
        [Route("Workplace/{taskId}")]
        public async Task<IActionResult> Workplace(int taskId)
        {
            var model = await BuildWorkplaceViewModelAsync(taskId);
            if (model == null) return NotFound();

            return View(model);
        }

        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public async Task<IActionResult> Details(int taskId)
        {
            var model = await BuildWorkplaceViewModelAsync(taskId, onlyForView: true);
            if (model == null) return NotFound();

            return View("Workplace", model);
        }

        [Authorize(Roles = "Worker,Admin")]
        [HttpPost]
        public async Task<IActionResult> StartTask(int taskId)
        {
            var task = await _context.ProductionTasks.FirstOrDefaultAsync(t => t.ProductionTaskId == taskId);
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
                StartTime = DateTime.Now
            };

            await _context.WorkSessions.AddAsync(newSession);
            await _context.SaveChangesAsync();

            return RedirectToAction("Workplace", new { TaskId = taskId });
        }

        [Authorize(Roles = "Worker,Admin")]
        [HttpPost]
        public async Task<IActionResult> StopTask(int sessionId)
        {
            var session = await _context.WorkSessions.FirstOrDefaultAsync(ws => ws.WorkSessionId == sessionId);
            if (session == null)
                return NotFound();

            session.EndTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("Workplace", new { TaskId = session.ProductionTaskId });
        }

        [Authorize(Roles = "Worker,Admin")]
        [HttpPost]
        public async Task<IActionResult> CompleteTask(int taskId)
        {
            var task = await _context.ProductionTasks.FirstOrDefaultAsync(t => t.ProductionTaskId == taskId);
            if (task == null)
                return NotFound();

            task.ActualEndTime = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("Workplace", new { TaskId = taskId });
        }

        [Authorize(Roles = "Director,Admin")]
        [HttpPost]
        public async Task<IActionResult> RemoveProduction(int ProductionId)
        {
            var prod = await _context.Productions.FirstOrDefaultAsync(w => w.ProductionId == ProductionId);
            var tasks = await _context.ProductionTasks.Where(w => w.ProductionId == ProductionId).ToListAsync();
            if (prod == null)
            {
                return NotFound();
            }

            foreach (var task in tasks)
            {
                _context.ProductionTasks.Remove(task);
            }

            _context.Productions.Remove(prod);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Production has been removed.";
            return RedirectToAction("ProductionList", new {prod.State});
        }
    }
}
