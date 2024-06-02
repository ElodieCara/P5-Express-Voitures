using ExpressVoitures.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;
using System.Threading.Tasks;
using ExpressVoitures.ViewModels;
using Express_Voitures.Models;
using ExpressVoitures.Models;

namespace ExpressVoitures.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, SignInManager<User> signInManager, ILogger<HomeController> logger)
        {
            _context = context;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var cars = await _context.Cars.Include(c => c.Make).Include(c => c.Model).ToListAsync();
            return View(cars);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            _logger.LogInformation("Navigating to Login page.");
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            _logger.LogInformation("Login attempt for user {Email}", model.Email);
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User {Email} logged in successfully.", model.Email);
                    return LocalRedirect(model.ReturnUrl);
                }
                _logger.LogWarning("Invalid login attempt for user {Email}.", model.Email);
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            else
            {
                _logger.LogWarning("Model state is not valid.");
            }
            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
