using CampusConnect.Models;
using CampusConnect.Repositories;
using CampusConnect.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace CampusConnect.Controllers
{
    public class QuestionsController : BaseController
    {
        private readonly IQuestionRepository _questionRepository;

        public QuestionsController(IQuestionRepository questionRepository, UserManager<ApplicationUser> userManager) : base(userManager)
        {
            _questionRepository = questionRepository;
        }

        public IActionResult Index(string? searchTerm, string? tag, string? status)
        {
            var allQuestions = _questionRepository.GetAllWithUsers(searchTerm, tag, status);
            ViewData["CurrentSearch"] = searchTerm;
            ViewData["CurrentTag"] = tag;
            ViewData["CurrentStatus"] = status;
            return View(allQuestions);
        }

        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateQuestionViewModel
            {
                // This call is now valid because the parameter is optional
                AllQuestions = _questionRepository.GetAllWithUsers()
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null) return Challenge();
                var question = new Question { 
                    Title = model.Title, 
                    Content = model.Content,
                    CreatedAt = DateTime.UtcNow, 
                    ApplicationUserId = userId 
                };
                _questionRepository.CreateQuestionWithTags(question, model.Tags);



                return RedirectToAction("Create");
            }
            model.AllQuestions = _questionRepository.GetAllWithUsers();
            return View(model);
        }

        public IActionResult Details(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null) return NotFound();
            return View(question);
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null) return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.ApplicationUserId != currentUserId) return Forbid();
            var tagsString = string.Join(", ", question.QuestionTags.Select(qt => qt.Tag.Name));
            var viewModel = new EditQuestionViewModel { 
                Id = question.Id, 
                Title = question.Title, 
                Content = question.Content,
                Tags = tagsString 
            };
            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditQuestionViewModel model)
        {
            if (id != model.Id) return BadRequest();
            if (ModelState.IsValid)
            {
                var originalQuestion = _questionRepository.GetById(id);
                if (originalQuestion == null) return NotFound();
                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (originalQuestion.ApplicationUserId != currentUserId) return Forbid();
                originalQuestion.Title = model.Title;
                originalQuestion.Content = model.Content;
                _questionRepository.Update(originalQuestion, model.Tags);
                return RedirectToAction("Details", new { id = model.Id });
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Delete(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null) return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.ApplicationUserId != currentUserId) return Forbid();
            return View(question);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null) return NotFound();
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.ApplicationUserId != currentUserId) return Forbid();
            _questionRepository.Delete(id);
            return RedirectToAction("Index", "Home");
        }
    }
}