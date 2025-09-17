using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CampusConnect.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Answer> GetAll()
        {
            return _context.Answers
                           .Include(a => a.ApplicationUser)
                           .Include(a => a.Question)
                           .ToList();
        }

        public Answer? GetById(int id)
        {
            return _context.Answers
                           .Include(a => a.ApplicationUser)
                           .Include(a => a.Question)
                           .FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Answer> GetByQuestionId(int questionId)
        {
            return _context.Answers
                           .Include(a => a.ApplicationUser)
                           .Where(a => a.QuestionId == questionId)
                           .ToList();
        }

        public void Add(Answer answer) => _context.Answers.Add(answer);

        public void Update(Answer answer) => _context.Answers.Update(answer);

        public void Remove(Answer answer) => _context.Answers.Remove(answer);

        public bool Exists(int id) => _context.Answers.Any(a => a.Id == id);

        public void Save() => _context.SaveChanges();
    }
}
