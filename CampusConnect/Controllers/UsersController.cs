using CampusConnect.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CampusConnect.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var users = await _userRepository.GetAllUsers();
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.FullName != null && s.FullName.Contains(searchString) || s.Email != null && s.Email.Contains(searchString));
            }
            return View(users);
        }
    }
}