using System.Collections.Generic;

namespace CampusConnect.Models.repositories
{
    public interface IAnswerRepository
    {
        IEnumerable<Answer> GetAll();
        Answer? GetById(int id);
        IEnumerable<Answer> GetByQuestionId(int questionId);
        void Add(Answer answer);
        void Update(Answer answer);
        void Remove(Answer answer);
        bool Exists(int id);
        void Save();
    }
}
