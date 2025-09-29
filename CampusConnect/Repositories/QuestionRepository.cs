using CampusConnect.Data;
using CampusConnect.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CampusConnect.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Question> GetAllWithUsers(string? searchTerm = null, string? tag = null, string? status = null)
        {
            var query = _context.Questions.Include(q => q.ApplicationUser).AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                query = query.Where(q =>
                    q.Title.Contains(searchTerm)
                );
            }

            if (!string.IsNullOrEmpty(tag))
            {
                query = query.Where(q => q.QuestionTags.Any(qt => qt.Tag.Name.Contains(tag)));
            }

            if (!string.IsNullOrEmpty(status))
            {
                if (status == "answered")
                {
                    query = query.Where(q => q.HasAcceptedAnswer);
                }
                else if (status == "unanswered")
                {
                    query = query.Where(q => !q.HasAcceptedAnswer);
                }
            }

            return query.OrderByDescending(q => q.CreatedAt).ToList();
        }

         public Question? GetById(int id)
  {
      return _context.Questions
          .Include(q => q.ApplicationUser)
          .Include(q => q.Answers)
              .ThenInclude(a => a.ApplicationUser)
          .Include(q => q.Answers)
              .ThenInclude(a => a.AnswerVotes)
          .Include(q => q.QuestionTags)
              .ThenInclude(qt => qt.Tag)
          .FirstOrDefault(q => q.Id == id);
  }

        public void CreateQuestionWithTags(Question question, string tagsString)
        {
            var tagNames = tagsString.Split(',').Select(t => t.Trim().ToLower()).Where(t => !string.IsNullOrEmpty(t)).Distinct();
            foreach (var tagName in tagNames)
            {
                var existingTag = _context.Tags.FirstOrDefault(t => t.Name == tagName);
                if (existingTag == null)
                {
                    existingTag = new Tag { Name = tagName };
                    _context.Tags.Add(existingTag);
                }
                question.QuestionTags.Add(new QuestionTag { Tag = existingTag });
            }
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public void Update(Question question, string tagsString)
        {
            var originalQuestion = _context.Questions.Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag).FirstOrDefault(q => q.Id == question.Id);
            if (originalQuestion == null) return;
            originalQuestion.Title = question.Title;
            var newTagNames = tagsString.Split(',').Select(t => t.Trim().ToLower()).Where(t => !string.IsNullOrEmpty(t)).ToHashSet();
            var currentTagNames = originalQuestion.QuestionTags.Select(qt => qt.Tag.Name).ToHashSet();
            var tagsToRemove = originalQuestion.QuestionTags.Where(qt => !newTagNames.Contains(qt.Tag.Name)).ToList();
            _context.QuestionTags.RemoveRange(tagsToRemove);
            var tagNamesToAdd = newTagNames.Where(n => !currentTagNames.Contains(n));
            foreach (var tagName in tagNamesToAdd)
            {
                var existingTag = _context.Tags.FirstOrDefault(t => t.Name == tagName);
                if (existingTag == null)
                {
                    existingTag = new Tag { Name = tagName };
                    _context.Tags.Add(existingTag);
                }
                originalQuestion.QuestionTags.Add(new QuestionTag { Tag = existingTag });
            }
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var question = _context.Questions.Find(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }
        }
        public async Task<IEnumerable<Question>> GetAll() 
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<IEnumerable<Question>> GetQuestionsByUser(string userId)
        {
            return await _context.Questions.Where(q => q.ApplicationUserId == userId).OrderByDescending(q => q.CreatedAt).Take(5).ToListAsync();
        }

        public async Task CreateBulk(List<Question> questions)
        {
            await _context.Questions.AddRangeAsync(questions);
            await _context.SaveChangesAsync();
        }
    }
}