using CampusConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusConnect.Repositories
{
    public interface IAnswerRepository
    {
        void CreateAnswer(Answer answer);
        Answer? GetById(int id);
        void Update(Answer answer);
        void Delete(int id);
        void MarkAsBest(Answer answer);
        Task<IEnumerable<Answer>> GetAnswersByQuestion(int questionId);
        Task<IEnumerable<Answer>> GetAnswersByUser(string userId);
        Task CreateBulk(List<Answer> answers);
    }
}