
using CampusConnect.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CampusConnect.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<ApplicationUser>> GetAllUsers();
    }
}
