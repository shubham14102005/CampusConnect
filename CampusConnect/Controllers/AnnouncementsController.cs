using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace CampusConnect.Controllers
{
    public class AnnouncementsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public AnnouncementsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var announcements = _context.Announcements.OrderByDescending(a => a.CreatedAt).ToList();
            return View(announcements);
        }

        [Authorize(Roles = "Faculty")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Faculty")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")] Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.CreatedAt = DateTime.UtcNow;
                _context.Add(announcement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(announcement);
        }
    }
}
