using CampusConnect.Models;
using CampusConnect.Repositories;
using CampusConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampusConnect.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IAnswerVoteRepo _answerVoteRepo;

        public UsersController(IUserRepository userRepository, UserManager<ApplicationUser> userManager, IQuestionRepository questionRepository, IAnswerRepository answerRepository, IAnswerVoteRepo answerVoteRepo) : base(userManager)
        {
            _userRepository = userRepository;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _answerVoteRepo = answerVoteRepo;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var users = await _userRepository.GetAllUsers();
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.FullName != null && s.FullName.ToLower().Contains(searchString.ToLower()));
            }

            users = users.OrderBy(u => u.FullName == "User Deleted").ThenBy(u => u.FullName);

            var userViewModels = new List<UserViewModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userViewModels.Add(new UserViewModel { User = user, Roles = roles });
            }

            return View(userViewModels);
        }

        public async Task<IActionResult> UserProfile(string id)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null) return NotFound();

            var questions = await _questionRepository.GetQuestionsByUser(id);
            var answers = await _answerRepository.GetAnswersByUser(id);

            var model = new UserProfileViewModel
            {
                User = user,
                QuestionCount = questions.Count(),
                AnswerCount = answers.Count(),
                RecentQuestions = questions,
                RecentAnswers = answers,
                Reputation = user.Reputation
            };

            return View(model);
        }

        [Authorize(Roles = "Faculty")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            if (!roles.Contains("Student") && roles.Any())
            {
                TempData["ErrorMessage"] = "Only student accounts or users with no role can be deleted by faculty.";
                return RedirectToAction(nameof(Index));
            }

            // Soft delete the user
            var random = new Random();
            var randomNumber = random.Next(1000, 9999);
            user.UserName = $"userdeleted{randomNumber}";
            user.Email = $"userdeleted{randomNumber}@deleted.com";
            user.FullName = "User Deleted";
            user.NormalizedUserName = user.UserName.ToUpper();
            user.NormalizedEmail = user.Email.ToUpper();

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                // Remove user from all roles
                var userRoles = await _userManager.GetRolesAsync(user);
                await _userManager.RemoveFromRolesAsync(user, userRoles);

                TempData["SuccessMessage"] = $"User account {user.Email} has been anonymized.";
            }
            else
            {
                TempData["ErrorMessage"] = $"Error anonymizing user account: {string.Join(", ", result.Errors.Select(e => e.Description))}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}