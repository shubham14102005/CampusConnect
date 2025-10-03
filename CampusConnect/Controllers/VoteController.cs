using CampusConnect.Hubs;
using CampusConnect.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

using CampusConnect.Models;
using Microsoft.AspNetCore.Identity;

namespace CampusConnect.Controllers
{
    [Authorize]
    public class VoteController : BaseController
    {
        private readonly IAnswerVoteRepo _voteRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public VoteController(IAnswerVoteRepo voteRepository, IAnswerRepository answerRepository, IHubContext<NotificationHub> hubContext, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _voteRepository = voteRepository;
            _answerRepository = answerRepository;
            _hubContext = hubContext;
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

            var answer = _answerRepository.GetById(answerId);
            if (answer == null) 
            {
                return NotFound();
            }

            await _hubContext.Clients.User(answer.ApplicationUserId).SendAsync("ReceiveNotification", $"Your answer to \"{answer.Question.Title}\" received a new vote!");

            return Json(new { upvotes = answer.UpvoteCount, downvotes = answer.DownvoteCount });
        }
    }
}