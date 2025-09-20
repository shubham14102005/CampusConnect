using CampusConnect.Data;
using CampusConnect.Models;
using System.Linq;

namespace CampusConnect.Repositories
{
    public class AnswerVoteRepository : IAnswerVoteRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswerVoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void AddOrUpdateVote(int answerId, string userId, bool isUpvote)
        {
            var answer = _context.Answers.Find(answerId);
            if (answer == null) return;

            var existingVote = _context.AnswerVotes
                .FirstOrDefault(v => v.AnswerId == answerId && v.ApplicationUserId == userId);

            if (existingVote == null)
            {
                var vote = new AnswerVote { AnswerId = answerId, ApplicationUserId = userId, IsUpvote = isUpvote };
                _context.AnswerVotes.Add(vote);
                answer.Score += isUpvote ? 1 : -1;
            }
            else if (existingVote.IsUpvote != isUpvote)
            {
                existingVote.IsUpvote = isUpvote;
                answer.Score += isUpvote ? 2 : -2;
            }
            else
            {
                _context.AnswerVotes.Remove(existingVote);
                answer.Score += isUpvote ? -1 : 1;
            }
            _context.SaveChanges();
        }
    }
}