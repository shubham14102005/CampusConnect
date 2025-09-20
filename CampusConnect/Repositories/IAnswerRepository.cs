using CampusConnect.Models;

namespace CampusConnect.Repositories
{
    public interface IAnswerRepository
    {
        void CreateAnswer(Answer answer);
        Answer? GetById(int id);
        void Update(Answer answer);
        void Delete(int id);
    }

    public interface IAnswerVoteRepository
    {
        void AddOrUpdateVote(int answerId, string userId, bool isUpvote);
    }
}