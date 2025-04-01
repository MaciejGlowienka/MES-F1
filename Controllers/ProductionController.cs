using MES_F1.Data;
using MES_F1.Models;
using MES_F1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            return RedirectToAction("ProductionList");
        }


        public IActionResult ProductionList()
        {
            ViewBag.Productions = null;
            return View();
        }

        [HttpPost]
        public IActionResult ProductionList(ProductionState state)
        {
            ViewBag.Productions = _context.Productions.Where(w => w.State == state).ToList();
            ViewBag.ProductionState = state;
            return View();
        }

        [HttpPost]
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
                State = production.State 
                
            };

            ViewBag.ProductionTasks = _context.ProductionTasks.Where(w => w.ProductionId == productionId).ToList();
            return View(model);
        }

        [HttpPost]
        public IActionResult ProductionEdit(int productionId, ProductionState State)
        {
            var prod = _context.Productions.FirstOrDefault(w => w.ProductionId == productionId);
            prod.State = State;
            _context.Productions.Update(prod);
            _context.SaveChanges();

            return RedirectToAction("ProductionList");
        }

        [HttpPost]
        public IActionResult ProductionSetupDisable(ProductionCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

           
            foreach (var taskModel in model.Tasks)
            {
                var task = new ProductionTask
                {
                    ProductionId = taskModel.ProductionId,
                    InstructionStep = taskModel.InstructionStep,
                    TaskName = taskModel.TaskName,
                    TeamId = taskModel.TeamId.Value
                };
               
                _context.ProductionTasks.Add(task);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
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
            return RedirectToAction("ProductionList");
        }
    }
}
