using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CampusConnect.Repositories
{
    public class AnswerVoteRepository : IAnswerVoteRepository
    {
        private readonly ApplicationDbContext _context;

        public AnswerVoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<AnswerVote> GetAll()
        {
            return _context.AnswerVotes
                           .Include(v => v.ApplicationUser)
                           .Include(v => v.Answer)
                           .ToList();
        }

        public AnswerVote? GetById(int id)
        {
            return _context.AnswerVotes
                           .Include(v => v.ApplicationUser)
                           .Include(v => v.Answer)
                           .FirstOrDefault(v => v.Id == id);
        }

        public IEnumerable<AnswerVote> GetByAnswerId(int answerId)
        {
            return _context.AnswerVotes
                           .Include(v => v.ApplicationUser)
                           .Where(v => v.AnswerId == answerId)
                           .ToList();
        }

        public void Add(AnswerVote vote) => _context.AnswerVotes.Add(vote);

        public void Update(AnswerVote vote) => _context.AnswerVotes.Update(vote);

        public void Remove(AnswerVote vote) => _context.AnswerVotes.Remove(vote);

        public bool Exists(int id) => _context.AnswerVotes.Any(v => v.Id == id);

        public void Save() => _context.SaveChanges();
    }
}
