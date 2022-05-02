using Microsoft.AspNetCore.Mvc;

namespace Facilis.ToDoApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}