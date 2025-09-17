using System.Collections.Generic;
using CampusConnect.Models;
using CampusConnect.Repositories;

namespace CampusConnect.Repositories
{
    public interface ITagRepository
    {
        IEnumerable<Tag> GetAll();
        Tag? GetById(int id);
        void Add(Tag tag);
        void Update(Tag tag);
        void Remove(Tag tag);
        bool Exists(int id);
        void Save();
    }
}
