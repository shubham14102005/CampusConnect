using CampusConnect.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var questions = await _context.Questions
                                          .Include(q => q.ApplicationUser)
                                          .OrderByDescending(q => q.CreatedAt)
                                          .ToListAsync();
            return View(questions);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}