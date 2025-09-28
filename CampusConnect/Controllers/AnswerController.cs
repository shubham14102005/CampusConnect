    using CampusConnect.Models;
    using CampusConnect.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Security.Claims;

    namespace CampusConnect.Controllers
    {
        [Authorize]
        public class AnswerController : Controller
        {
            private readonly IAnswerRepository _answerRepository;
            private readonly UserManager<ApplicationUser> _userManager;

            public AnswerController(IAnswerRepository answerRepository, UserManager<ApplicationUser> userManager)
            {
                _answerRepository = answerRepository;
                _userManager = userManager;
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Create(int questionId, string content)
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    TempData["ErrorMessage"] = "Answer content cannot be empty.";
                    return RedirectToAction("Details", "Questions", new { id = questionId });
                }
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Challenge();
                var answer = new Answer { Content = content, CreatedAt = DateTime.UtcNow, QuestionId = questionId, ApplicationUserId = userId };
                _answerRepository.CreateAnswer(answer);
                return RedirectToAction("Details", "Questions", new { id = questionId });
            }

            public IActionResult Edit(int id)
            {
                var answer = _answerRepository.GetById(id);
                if (answer == null) return NotFound();
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (answer.ApplicationUserId != currentUserId) return Forbid();
                return View(answer);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Edit(Answer model)
            {
               // if (!ModelState.IsValid) return View(model);
                var originalAnswer = _answerRepository.GetById(model.Id);
                if (originalAnswer == null) return NotFound();
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (originalAnswer.ApplicationUserId != currentUserId) return Forbid();
                originalAnswer.Content = model.Content;
                _answerRepository.Update(originalAnswer);
                return RedirectToAction("Details", "Questions", new { id = originalAnswer.QuestionId });
            }

            // GET: /Answer/Delete/5
            public IActionResult Delete(int id)
            {
                var answer = _answerRepository.GetById(id);
                if (answer == null) return NotFound();

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (answer.ApplicationUserId != currentUserId)
                {
                    return Forbid();
                }

                return View(answer);
            }

            // POST: /Answer/Delete/5
            [HttpPost, ActionName("Delete")]
            [ValidateAntiForgeryToken]
            public IActionResult DeleteConfirmed(int id)
            {
                var answer = _answerRepository.GetById(id);
                if (answer == null) return NotFound();

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (answer.ApplicationUserId != currentUserId)
                {
                    return Forbid();
                }

                _answerRepository.Delete(id);

                return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
            }
        }
    }