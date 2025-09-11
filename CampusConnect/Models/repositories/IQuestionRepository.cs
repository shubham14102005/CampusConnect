using System.Collections.Generic;

namespace CampusConnect.Models.repositories
{
    public interface IQuestionRepository
    {
        IEnumerable<Question> GetAll();
        Question? GetById(int id);
        void Add(Question question);
        void Update(Question question);
        void Remove(Question question);
        bool Exists(int id);
        void Save();
    }
}
