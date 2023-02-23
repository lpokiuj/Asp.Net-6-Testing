using Microsoft.AspNetCore.Mvc;

namespace testt.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
