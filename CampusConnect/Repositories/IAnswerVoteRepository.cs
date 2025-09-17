using System.Collections.Generic;
using CampusConnect.Models;
using CampusConnect.Repositories;

namespace CampusConnect.Repositories
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
