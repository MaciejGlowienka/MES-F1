using MES_F1.Data;
using MES_F1.Models;
using MES_F1.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        public IActionResult ProductionSetup(int InstructionId)
        {
            ViewBag.InstructionId = InstructionId;
            ViewBag.InstructionSteps = _context.InstructionSteps.Where(w => w.InstructionId == InstructionId).ToList();
            ViewBag.Teams = _context.Teams.ToList();
            ViewBag.Machines = _context.Machines.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult ProductionCreate(ProductionCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            Instruction instruction = _context.Instructions.FirstOrDefault(w => w.InstructionId == model.InstructionId);
            if (instruction == null)
            {
                return NotFound("Nie znaleziono instrukcji.");
            }

            Production prod = new Production() 
            {
                StartTime = DateTime.Now,
                InstructionId = instruction.InstructionId,
                Name = instruction.InstructionName + " " + DateTime.Now
            };
            
            _context.Productions.Add(prod);
            _context.SaveChanges();
            _context.Entry(prod).GetDatabaseValues();

            var prodId = prod.ProductionId;

            foreach (var taskModel in model.Tasks)
            {
                var task = new ProductionTask
                {
                    ProductionId = prodId,
                    InstructionStep = taskModel.InstructionStep,
                    TaskName = taskModel.TaskName,
                    TeamId = taskModel.TeamId.Value
                };
               
                _context.ProductionTasks.Add(task);
            }

            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
