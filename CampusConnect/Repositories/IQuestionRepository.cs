using CampusConnect.Models;
using System.Collections.Generic;

namespace CampusConnect.Repositories
{
    public interface IQuestionRepository
    {
        Question CreateQuestionWithTags(Question question, string tags);
        List<Question> GetAllWithUsers();
        Question? GetById(int id); // ADD a '?' here to fix the warning
        void Update(Question question, string tagsString);
        void Delete(int id);
    }
}