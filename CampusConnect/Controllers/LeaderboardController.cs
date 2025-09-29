
using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;

namespace CampusConnect.Controllers
{
    public class LeaderboardController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardController(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _context.Users
                .OrderByDescending(u => u.Reputation)
                .ToListAsync();

            // This is a simple way to calculate reputation.
            // A more efficient way would be to have a scheduled job to update the reputation.
            foreach (var user in users)
            {
                var answerUpvotes = await _context.AnswerVotes
                    .CountAsync(av => av.Answer.ApplicationUserId == user.Id && av.IsUpVote);
                var answerDownvotes = await _context.AnswerVotes
                    .CountAsync(av => av.Answer.ApplicationUserId == user.Id && !av.IsUpVote);

                    user.Reputation = answerUpvotes - answerDownvotes;
            }

            await _context.SaveChangesAsync();

            // Re-order after calculating reputation
            var leaderboard = users.OrderByDescending(u => u.Reputation).ToList();

            return View(leaderboard);
        }
    }
}
