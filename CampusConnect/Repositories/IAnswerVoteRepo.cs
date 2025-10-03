using System.Threading.Tasks;

namespace CampusConnect.Repositories
{
    public interface IAnswerVoteRepo
    {
        Task AddOrUpdateVote(int answerId, string userId, bool isUpvote);
        Task DeleteVotesByAnswer(int answerId);
    }
}