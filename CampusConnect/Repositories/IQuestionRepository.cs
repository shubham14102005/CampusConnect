using CampusConnect.Data;
using CampusConnect.Models;
using CampusConnect.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CampusConnect.Repositories
{
    public interface IQuestionRepository
{
    IEnumerable<Question> GetAll();
    Question? GetById(int id);
    void Add(Question question);
    void Update(Question question);
    void Delete(int id);
}

}
