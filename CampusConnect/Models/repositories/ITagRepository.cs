using System.Collections.Generic;

namespace CampusConnect.Models.repositories
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
