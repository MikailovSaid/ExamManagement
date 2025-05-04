using ExamManagement.Entities;
using ExamManagement.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ExamManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userRepository.GetByUsernameAsync(username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                ViewBag.Error = "Username or password is incorrect";
                return View();
            }

            HttpContext.Session.SetString("Username", user.Username);
            return RedirectToAction("Index", "Dashboard");
        }

        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var existing = await _userRepository.GetByUsernameAsync(username);
            if (existing != null)
            {
                ViewBag.Error = "Username already exists.";
                return View();
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var newUser = new User
            {
                Username = username,
                Password = hashedPassword
            };

            await _userRepository.AddAsync(newUser);
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
