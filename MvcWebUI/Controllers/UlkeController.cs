using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    public class UlkeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
