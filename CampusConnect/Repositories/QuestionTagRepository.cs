using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CampusConnect.Repositories
{
    public class QuestionTagRepository : IQuestionTagRepository
    {
        private readonly ApplicationDbContext _context;

        public QuestionTagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<QuestionTag> GetAll()
        {
            return _context.QuestionTags
                           .Include(qt => qt.Question)
                           .Include(qt => qt.Tag)
                           .ToList();
        }

        public IEnumerable<QuestionTag> GetByQuestionId(int questionId)
        {
            return _context.QuestionTags
                           .Include(qt => qt.Tag)
                           .Where(qt => qt.QuestionId == questionId)
                           .ToList();
        }

        public IEnumerable<QuestionTag> GetByTagId(int tagId)
        {
            return _context.QuestionTags
                           .Include(qt => qt.Question)
                           .Where(qt => qt.TagId == tagId)
                           .ToList();
        }

        public void Add(QuestionTag questionTag) => _context.QuestionTags.Add(questionTag);

        public void Remove(QuestionTag questionTag) => _context.QuestionTags.Remove(questionTag);

        public bool Exists(int questionId, int tagId)
        {
            return _context.QuestionTags.Any(qt => qt.QuestionId == questionId && qt.TagId == tagId);
        }

        public void Save() => _context.SaveChanges();
    }
}
