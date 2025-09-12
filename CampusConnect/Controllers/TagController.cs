using CampusConnect.Models;
using CampusConnect.Models.repositories;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IQuestionTagRepository _questionTagRepository;
        private readonly IQuestionRepository _questionRepository;

        public TagsController(ITagRepository tagRepository,
                              IQuestionTagRepository questionTagRepository,
                              IQuestionRepository questionRepository)
        {
            _tagRepository = tagRepository;
            _questionTagRepository = questionTagRepository;
            _questionRepository = questionRepository;
        }

        // GET: Tags
        public IActionResult Index()
        {
            var tags = _tagRepository.GetAll();
            return View(tags);
        }

        // GET: Tags/Details/5
        public IActionResult Details(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag == null) return NotFound();

            var taggedQuestions = _questionTagRepository.GetByTagId(id);
            ViewBag.Questions = taggedQuestions;
            return View(tag);
        }

        // GET: Tags/Create
        public IActionResult Create() => View();

        // POST: Tags/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Name")] Tag tag)
        {
            if (ModelState.IsValid)
            {
                _tagRepository.Add(tag);
                _tagRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Edit/5
        public IActionResult Edit(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        // POST: Tags/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name")] Tag tag)
        {
            if (id != tag.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _tagRepository.Update(tag);
                _tagRepository.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(tag);
        }

        // GET: Tags/Delete/5
        public IActionResult Delete(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        // POST: Tags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var tag = _tagRepository.GetById(id);
            if (tag != null)
            {
                _tagRepository.Remove(tag);
                _tagRepository.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Tags/Assign
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Assign(int questionId, int tagId)
        {
            if (!_questionTagRepository.Exists(questionId, tagId))
            {
                var qt = new QuestionTag
                {
                    QuestionId = questionId,
                    TagId = tagId
                };
                _questionTagRepository.Add(qt);
                _questionTagRepository.Save();
            }

            return RedirectToAction("Details", "Questions", new { id = questionId });
        }

        // POST: Tags/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int questionId, int tagId)
        {
            var qt = _questionTagRepository.GetByQuestionId(questionId)
                                           .FirstOrDefault(x => x.TagId == tagId);
            if (qt != null)
            {
                _questionTagRepository.Remove(qt);
                _questionTagRepository.Save();
            }

            return RedirectToAction("Details", "Questions", new { id = questionId });
        }
    }
}
