using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void CreateAnswer(Answer answer)
        {
            _context.Answers.Add(answer);
            _context.SaveChanges();
        }

        public Answer? GetById(int id)
        {
            return _context.Answers
                .Include(a => a.ApplicationUser)
                .Include(a => a.Question)
                .FirstOrDefault(a => a.Id == id);
        }

        public void Update(Answer answer)
        {
            // 1. Find the original answer in the database
            var originalAnswer = _context.Answers.Find(answer.Id);

            if (originalAnswer != null)
            {
                // 2. Update only the properties that should be changed
                originalAnswer.Content = answer.Content;

                // 3. Save the changes to the original record
                _context.SaveChanges();
            }
        }
        public void Delete(int id)
        {
            var answer = _context.Answers.Find(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                _context.SaveChanges();
            }
        }
        public async Task<IEnumerable<Answer>> GetAnswersByQuestion(int questionId)
        {
            return await _context.Answers.Where(a => a.QuestionId == questionId).ToListAsync();
        }

        public async Task<IEnumerable<Answer>> GetAnswersByUser(string userId)
        {
            return await _context.Answers.Where(a => a.ApplicationUserId == userId).OrderByDescending(a => a.CreatedAt).Take(5).ToListAsync();
        }

        public async Task CreateBulk(List<Answer> answers)
        {
            await _context.Answers.AddRangeAsync(answers);
            await _context.SaveChangesAsync();
        }

        public void MarkAsBest(Answer answer)
        {
            var question = _context.Questions.Include(q => q.Answers).FirstOrDefault(q => q.Id == answer.QuestionId);
            if (question == null) return;

            foreach (var otherAnswer in question.Answers)
            {
                otherAnswer.IsBestAnswer = false;
            }

            answer.IsBestAnswer = true;
            question.HasAcceptedAnswer = true;

            _context.SaveChanges();
        }
    }
}