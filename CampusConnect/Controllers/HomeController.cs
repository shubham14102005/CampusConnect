using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Content("CampusConnect is running!");
        }
    }
}
