using CampusConnect.Models;
using CampusConnect.Repositories;
using CampusConnect.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CampusConnect.Controllers
{
    public class AccountController : BaseController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IQuestionRepository _questionRepository;
        private readonly IAnswerRepository _answerRepository;
        private readonly IUserRepository _userRepository;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IQuestionRepository questionRepository, IAnswerRepository answerRepository, IUserRepository userRepository) : base(userManager)
        {
            _signInManager = signInManager;
            _questionRepository = questionRepository;
            _answerRepository = answerRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Profile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return Challenge();
            }

            var user = await _userRepository.GetUserById(userId);
            if (user == null) return NotFound();

            var questions = await _questionRepository.GetQuestionsByUser(userId);
            var answers = await _answerRepository.GetAnswersByUser(userId);

            var model = new ProfileViewModel
            {
                QuestionCount = questions.Count(),
                AnswerCount = answers.Count(),
                RecentQuestions = questions,
                RecentAnswers = answers,
                Reputation = user.Reputation
            };

            return View(model);
        }

        // GET: /Account/Edit
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var model = new EditProfileViewModel
            {
                FullName = user.FullName
            };

            return View(model);
        }

        // POST: /Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            user.FullName = model.FullName;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Profile));
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }



        // GET: Login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login form submission
        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await _signInManager.PasswordSignInAsync(email, password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                // Redirect to a dashboard or home page after login
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View();
        }

        // GET: Register page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register form submission
        [HttpPost]
        public async Task<IActionResult> Register(string fullName, string email, string password)
        {
            if (!email.EndsWith("@ddu.ac.in"))
            {
                ModelState.AddModelError(string.Empty, "Only DDU students can register.");
                return View();
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Student");
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Dashboard", "Home"); // Redirect to dashboard after successful registration
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }

        // GET: /Account/RegisterFaculty
        [HttpGet]
        public IActionResult RegisterFaculty()
        {
            return View();
        }

        // POST: /Account/RegisterFaculty
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterFaculty(RegisterFacultyViewModel model)
        {
            if (!model.Email.EndsWith("@faculty.ddu.ac.in"))
            {
                ModelState.AddModelError(string.Empty, "Only DDU faculty can register.");
                return View(model);
            }

            if (model.SecretKey != "faculty @123")
            {
                ModelState.AddModelError(string.Empty, "Invalid secret key.");
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                FunctioningTitle = model.FunctioningTitle
            };

            if (model.Password == null)
            {
                ModelState.AddModelError(string.Empty, "Password is required.");
                return View(model);
            }

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Faculty");
                await _signInManager.SignInAsync(user, isPersistent: true);
                return RedirectToAction("Dashboard", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        // POST: Logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
