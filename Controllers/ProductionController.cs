using MES_F1.Data;
using MES_F1.Models;
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
        public IActionResult ProductionCreate(List<ProductionTask> tasks, int InstructionId)
        {
            Instruction instruction = _context.Instructions.FirstOrDefault(w => w.InstructionId == InstructionId);
            Production prod = new Production();
            prod.StartTime = DateTime.Now;
            prod.InstructionId = instruction.InstructionId;
            prod.Name = instruction.InstructionName + prod.StartTime;
            _context.Productions.Add(prod);
            foreach (var task in tasks)
            {
                _context.ProductionTasks.Add(task);
            }
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
