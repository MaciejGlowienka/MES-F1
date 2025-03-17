using Microsoft.AspNetCore.Mvc;

namespace MES_F1.Controllers
{
    public class WarehouseController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
