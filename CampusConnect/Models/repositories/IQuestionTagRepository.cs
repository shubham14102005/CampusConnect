using System.Collections.Generic;

namespace CampusConnect.Models.repositories
{
    public interface IQuestionTagRepository
    {
        IEnumerable<QuestionTag> GetAll();
        IEnumerable<QuestionTag> GetByQuestionId(int questionId);
        IEnumerable<QuestionTag> GetByTagId(int tagId);
        void Add(QuestionTag questionTag);
        void Remove(QuestionTag questionTag);
        bool Exists(int questionId, int tagId);
        void Save();
    }
}
