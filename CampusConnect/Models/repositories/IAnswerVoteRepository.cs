using System.Collections.Generic;

namespace CampusConnect.Models.repositories
{
    public interface IAnswerVoteRepository
    {
        IEnumerable<AnswerVote> GetAll();
        AnswerVote? GetById(int id);
        IEnumerable<AnswerVote> GetByAnswerId(int answerId);
        void Add(AnswerVote vote);
        void Update(AnswerVote vote);
        void Remove(AnswerVote vote);
        bool Exists(int id);
        void Save();
    }
}
