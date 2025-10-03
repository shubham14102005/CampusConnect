using Microsoft.AspNetCore.Mvc;
using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.ViewModels; 
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;


using Microsoft.AspNetCore.Identity;

namespace CampusConnect.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string tag)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var currentUser = await _context.Users.FindAsync(userId);

            var trendingQuestions = _context.Questions
                                            .Include(q => q.ApplicationUser)
                                            .Include(q => q.Answers)
                                            .AsQueryable();

            if (!string.IsNullOrEmpty(tag))
            {
                trendingQuestions = trendingQuestions.Where(q => q.QuestionTags.Any(qt => qt.Tag.Name == tag));
            }

            var model = new DashboardViewModel
            {
                CurrentUser = currentUser,
                SuggestedUsers = _context.Users
                                         .OrderBy(u => u.FullName)
                                         .Take(5)
                                         .ToList(),

                TrendingQuestions = trendingQuestions
                                            .OrderByDescending(q => q.CreatedAt)
                                            .Take(5)
                                            .ToList(),

                Announcements = _context.Announcements
                                          .OrderByDescending(a => a.CreatedAt)
                                          .Take(4)
                                          .ToList()
            };

            ViewData["CurrentTag"] = tag;
            return View("Dashboard", model);
        }
    }
}
