using CampusConnect.Models;
using CampusConnect.Models.repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CampusConnect.Controllers
{
    public class AnswersController : Controller
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IAnswerVoteRepository _answerVoteRepository;
        private readonly IQuestionRepository _questionRepository;

        public AnswersController(IAnswerRepository answerRepository,
                                 IAnswerVoteRepository answerVoteRepository,
                                 IQuestionRepository questionRepository)
        {
            _answerRepository = answerRepository;
            _answerVoteRepository = answerVoteRepository;
            _questionRepository = questionRepository;
        }

        // GET: Answers for a question
        public IActionResult Index(int questionId)
        {
            var answers = _answerRepository.GetByQuestionId(questionId);
            ViewBag.Question = _questionRepository.GetById(questionId);
            return View(answers);
        }

        // GET: Answers/Details/5
        public IActionResult Details(int id)
        {
            var answer = _answerRepository.GetById(id);
            if (answer == null) return NotFound();
            return View(answer);
        }

        // GET: Answers/Create
        public IActionResult Create(int questionId)
        {
            ViewData["QuestionId"] = new SelectList(new[] { new { Id = questionId } }, "Id", "Id");
            return View(new Answer { QuestionId = questionId });
        }

        // POST: Answers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,Content,CreatedAt,ApplicationUserId,QuestionId")] Answer answer)
        {
            if (ModelState.IsValid)
            {
                _answerRepository.Add(answer);
                _answerRepository.Save();
                return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
            }
            return View(answer);
        }

        // GET: Answers/Edit/5
        public IActionResult Edit(int id)
        {
            var answer = _answerRepository.GetById(id);
            if (answer == null) return NotFound();
            return View(answer);
        }

        // POST: Answers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Content,CreatedAt,ApplicationUserId,QuestionId")] Answer answer)
        {
            if (id != answer.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _answerRepository.Update(answer);
                _answerRepository.Save();
                return RedirectToAction("Details", "Questions", new { id = answer.QuestionId });
            }
            return View(answer);
        }

        // GET: Answers/Delete/5
        public IActionResult Delete(int id)
        {
            var answer = _answerRepository.GetById(id);
            if (answer == null) return NotFound();
            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var answer = _answerRepository.GetById(id);
            if (answer != null)
            {
                _answerRepository.Remove(answer);
                _answerRepository.Save();
            }
            return RedirectToAction(nameof(Index), new { questionId = answer?.QuestionId });
        }

        // POST: Answers/Vote
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Vote(int answerId, string userId, int value)
        {
            var existingVote = _answerVoteRepository.GetByAnswerId(answerId)
                                                    .FirstOrDefault(v => v.ApplicationUserId == userId);

            if (existingVote == null)
            {
                var newVote = new AnswerVote
                {
                    AnswerId = answerId,
                    ApplicationUserId = userId,
                    Value = value
                };
                _answerVoteRepository.Add(newVote);
            }
            else
            {
                existingVote.Value = value;
                _answerVoteRepository.Update(existingVote);
            }

            _answerVoteRepository.Save();

            return RedirectToAction("Details", new { id = answerId });
        }
    }
}
