using Microsoft.AspNetCore.Mvc;
using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.ViewModels; 
using Microsoft.EntityFrameworkCore;
using System.Linq;


namespace CampusConnect.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = new DashboardViewModel
            {
                // Suggested users (top 5 by FullName as example)
                SuggestedUsers = _context.Users
                                         .OrderBy(u => u.FullName)
                                         .Take(5)
                                         .ToList(),

                // Trending questions (top 5 latest questions)
                TrendingQuestions = _context.Questions
                                            .Include(q => q.ApplicationUser)
                                            .Include(q => q.Answers)
                                            .OrderByDescending(q => q.CreatedAt) // adjust field as needed
                                            .Take(5)
                                            .ToList()
            };

            // Return the Dashboard view in Home folder
            return View("Dashboard", model);
        }
    }
}
