using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace CampusConnect.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ApplicationDbContext _context;

        public TagRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Tag> GetAll() => _context.Tags.ToList();

        public Tag? GetById(int id) => _context.Tags.FirstOrDefault(t => t.Id == id);

        public void Add(Tag tag) => _context.Tags.Add(tag);

        public void Update(Tag tag) => _context.Tags.Update(tag);

        public void Remove(Tag tag) => _context.Tags.Remove(tag);

        public bool Exists(int id) => _context.Tags.Any(t => t.Id == id);

        public void Save() => _context.SaveChanges();
    }
}
