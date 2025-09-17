using System.Collections.Generic;
using CampusConnect.Models;
using CampusConnect.Repositories;

namespace CampusConnect.Repositories
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
