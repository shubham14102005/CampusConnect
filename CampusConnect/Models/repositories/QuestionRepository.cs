using CampusConnect.Data;
using CampusConnect.Models.repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CampusConnect.Models.repositories
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
                           .Include(q => q.QuestionTags)
                               .ThenInclude(qt => qt.Tag)
                           .ToList();
        }

        public Question? GetById(int id)
        {
            return _context.Questions
                           .Include(q => q.ApplicationUser)
                           .Include(q => q.QuestionTags)
                               .ThenInclude(qt => qt.Tag)
                           .FirstOrDefault(q => q.Id == id);
        }

        public void Add(Question question) => _context.Questions.Add(question);

        public void Update(Question question) => _context.Questions.Update(question);

        public void Remove(Question question) => _context.Questions.Remove(question);

        public bool Exists(int id) => _context.Questions.Any(q => q.Id == id);

        public void Save() => _context.SaveChanges();
    }
}
