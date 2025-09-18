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
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsController(IQuestionRepository questionRepository, UserManager<ApplicationUser> userManager)
        {
            _questionRepository = questionRepository;
            _userManager = userManager;
        }

        // GET: /Questions/Create
        [Authorize]
        public IActionResult Create()
        {
            var viewModel = new CreateQuestionViewModel
            {
                AllQuestions = _questionRepository.GetAllWithUsers()
            };
            return View(viewModel);
        }

        // POST: /Questions/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreateQuestionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (userId == null)
                {
                    return Challenge();
                }

                var question = new Question
                {
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

        // GET: /Questions/Details/5
        public IActionResult Details(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null)
            {
                return NotFound();
            }
            return View(question);
        }

        // GET: /Questions/Edit/5
        [Authorize]
        public IActionResult Edit(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question == null)
            {
                return NotFound();
            }

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.ApplicationUserId != currentUserId)
            {
                return Forbid();
            }

            var tagsString = string.Join(", ", question.QuestionTags.Select(qt => qt.Tag.Name));
            var viewModel = new EditQuestionViewModel
            {
                Id = question.Id,
                Title = question.Title,
                Content = question.Content,
                Tags = tagsString
            };
            return View(viewModel);
        }

        // POST: /Questions/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, EditQuestionViewModel model)
        {
            if (id != model.Id)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                var originalQuestion = _questionRepository.GetById(id);
                if (originalQuestion == null)
                {
                    return NotFound();
                }

                var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (originalQuestion.ApplicationUserId != currentUserId)
                {
                    return Forbid();
                }

                originalQuestion.Title = model.Title;
                originalQuestion.Content = model.Content;

                _questionRepository.Update(originalQuestion, model.Tags);
                return RedirectToAction("Details", new { id = model.Id });
            }

            return View(model);
        }
    }
}