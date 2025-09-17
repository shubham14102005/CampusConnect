using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Repositories;
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

        public IEnumerable<Question> GetAll()
        {
            return _context.Questions
                .Include(q => q.ApplicationUser)
                .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag)
                .Include(q => q.Answers);
        }

        public Question? GetById(int id)
        {
            return _context.Questions
                .Include(q => q.ApplicationUser)
                .Include(q => q.QuestionTags).ThenInclude(qt => qt.Tag)
                .Include(q => q.Answers).ThenInclude(a => a.ApplicationUser)
                .FirstOrDefault(q => q.Id == id);
        }

        public void Add(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public void Update(Question question)
        {
            _context.Questions.Update(question);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var q = _context.Questions.Find(id);
            if (q != null)
            {
                _context.Questions.Remove(q);
                _context.SaveChanges();
            }
        }
    }
}
