using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Repositories
{
    public class AnswerVoteRepo : IAnswerVoteRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswerVoteRepo(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task AddOrUpdateVote(int answerId, string userId, bool isUpvote)
        {
            var answer = await _context.Answers.FindAsync(answerId);
            if (answer == null) return;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return;
            var isFaculty = await _userManager.IsInRoleAsync(user, "Faculty");
            var voteWeight = isFaculty ? 10 : 1;

            var existingVote = await _context.AnswerVotes
                .FirstOrDefaultAsync(v => v.AnswerId == answerId && v.ApplicationUserId == userId);

            if (existingVote == null)
            {
                var vote = new AnswerVote { AnswerId = answerId, ApplicationUserId = userId, IsUpVote = isUpvote };
                _context.AnswerVotes.Add(vote);
                if (isUpvote) answer.UpvoteCount += voteWeight;
                else answer.DownvoteCount += voteWeight;
            }
            else if (existingVote.IsUpVote != isUpvote)
            {
                existingVote.IsUpVote = isUpvote;
                if (isUpvote)
                {
                    answer.UpvoteCount += voteWeight;
                    answer.DownvoteCount -= voteWeight;
                }
                else
                {
                    answer.UpvoteCount -= voteWeight;
                    answer.DownvoteCount += voteWeight;
                }
            }
            else
            {
                _context.AnswerVotes.Remove(existingVote);
                if (isUpvote) answer.UpvoteCount -= voteWeight;
                else answer.DownvoteCount -= voteWeight;
            }
            await _context.SaveChangesAsync();
        }
    }
}