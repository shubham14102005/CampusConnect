using CampusConnect.Models;
using CampusConnect.Models.repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace CampusConnect.Controllers
{
    [Authorize] // require login for all actions
    public class QuestionsController : Controller
    {
        private readonly IQuestionRepository _questionRepository;
        private readonly IQuestionTagRepository _questionTagRepository;
        private readonly ITagRepository _tagRepository;

        public QuestionsController(
            IQuestionRepository questionRepository,
            IQuestionTagRepository questionTagRepository,
            ITagRepository tagRepository)
        {
            _questionRepository = questionRepository;
            _questionTagRepository = questionTagRepository;
            _tagRepository = tagRepository;
        }

        // GET: Questions
        [AllowAnonymous]
        public IActionResult Index()
        {
            var questions = _questionRepository.GetAll();
            return View(questions);
        }

        // GET: Questions/Details/5
        [AllowAnonymous]
        public IActionResult Details(int? id)
        {
            if (id == null) return NotFound();

            var question = _questionRepository.GetById(id.Value);
            if (question == null) return NotFound();

            var assignedTags = _questionTagRepository.GetByQuestionId(id.Value);
            ViewBag.AssignedTags = assignedTags.Select(qt => qt.Tag);

            var assignedIds = assignedTags.Select(qt => qt.TagId).ToList();
            ViewBag.AvailableTags = _tagRepository.GetAll()
                                                 .Where(t => !assignedIds.Contains(t.Id));

            return View(question);
        }

        // GET: Questions/Create
        public IActionResult Create()
        {
            ViewBag.Tags = _tagRepository.GetAll();
            return View();
        }


        // POST: Questions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Title,Content")] Question question, string TagNames)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                question.ApplicationUserId = userId;
                question.CreatedAt = DateTime.UtcNow;

                _questionRepository.Add(question);
                _questionRepository.Save();

                if (!string.IsNullOrWhiteSpace(TagNames))
                {
                    var tagNames = TagNames.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                           .Select(t => t.Trim())
                                           .Distinct();

                    foreach (var tagName in tagNames)
                    {
                        var existingTag = _tagRepository.GetAll()
                                                        .FirstOrDefault(t => t.Name.Equals(tagName, StringComparison.OrdinalIgnoreCase));

                        Tag tagToUse;
                        if (existingTag != null)
                        {
                            tagToUse = existingTag;
                        }
                        else
                        {
                            tagToUse = new Tag { Name = tagName };
                            _tagRepository.Add(tagToUse);
                            _tagRepository.Save();
                        }

                        var questionTag = new QuestionTag
                        {
                            QuestionId = question.Id,
                            TagId = tagToUse.Id
                        };

                        _questionTagRepository.Add(questionTag);
                        _questionTagRepository.Save();
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(question);
        }


        // GET: Questions/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var question = _questionRepository.GetById(id.Value);
            if (question == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.ApplicationUserId != userId)
                return Forbid();

            return View(question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Title,Content")] Question question)
        {
            if (id != question.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existing = _questionRepository.GetById(id);
                if (existing == null) return NotFound();

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (existing.ApplicationUserId != userId)
                    return Forbid();

                existing.Title = question.Title;
                existing.Content = question.Content;

                _questionRepository.Update(existing);
                _questionRepository.Save();

                return RedirectToAction(nameof(Index));
            }
            return View(question);
        }

        // GET: Questions/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();

            var question = _questionRepository.GetById(id.Value);
            if (question == null) return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (question.ApplicationUserId != userId)
                return Forbid();

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var question = _questionRepository.GetById(id);
            if (question != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (question.ApplicationUserId != userId)
                    return Forbid();

                _questionRepository.Remove(question);
                _questionRepository.Save();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
