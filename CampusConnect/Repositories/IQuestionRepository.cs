using CampusConnect.Models;
using System.Collections.Generic;

namespace CampusConnect.Repositories
{
    public interface IQuestionRepository
    {
        void CreateQuestionWithTags(Question question, string tags);
        // FIX IS HERE: Add the optional search term parameter
        List<Question> GetAllWithUsers(string? searchTerm = null);
        Question? GetById(int id);
        void Update(Question question, string tagsString);
        void Delete(int id);
    Task<IEnumerable<Question>> GetQuestionsByUser(string userId);
    }
}