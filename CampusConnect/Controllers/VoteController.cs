using CampusConnect.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusConnect.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private readonly IAnswerVoteRepository _voteRepository;

        public VoteController(IAnswerVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        [HttpPost]
        public IActionResult Submit(int answerId, bool isUpvote, int questionId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId != null)
            {
                _voteRepository.AddOrUpdateVote(answerId, userId, isUpvote);
            }
            return RedirectToAction("Details", "Questions", new { id = questionId });
        }
    }
}