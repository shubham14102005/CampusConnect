using CampusConnect.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CampusConnect.Controllers
{
    [Authorize]
    public class VoteController : Controller
    {
        private readonly IAnswerVoteRepo _voteRepository;
        private readonly IAnswerRepository _answerRepository;

        public VoteController(IAnswerVoteRepo voteRepository, IAnswerRepository answerRepository)
        {
            _voteRepository = voteRepository;
            _answerRepository = answerRepository;
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int answerId, bool isUpvote)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) 
            {
                return Unauthorized();
            }

            await _voteRepository.AddOrUpdateVote(answerId, userId, isUpvote);

            // Assuming you have a method to get the updated counts
            var answer = _answerRepository.GetById(answerId); // You need to inject IAnswerRepository
            if (answer == null) 
            {
                return NotFound();
            }

            return Json(new { upvotes = answer.UpvoteCount, downvotes = answer.DownvoteCount });
        }
    }
}