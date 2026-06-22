using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DentalClinic.Models;
using DentalClinic.Models.DTO;

namespace DentalClinic.Controllers
{
    [Route("route")]
    public class AuthController : BaseController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;

        public AuthController(
            ILogger<AuthController> logger,
            DatabaseContext context,
            UserManager<Profile> userManager,
            SignInManager<Profile> signInManager) : base(context)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if (!User.Identity!.IsAuthenticated)
                return RedirectToAction("LoginPage");

            var profile = GetProfile();

            if (profile.IsAdmin)    return RedirectToAction("Index", "Admin");
            if (profile.IsManager)  return RedirectToAction("Index", "Manager");
            if (profile.IsDoctor)   return RedirectToAction("Index", "Doctor");
            return RedirectToAction("Index", "Client");
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Registration", model);

            var role = _context.Roles.FirstOrDefault(r => r.Name == "Клиент");
            if (role is null)
            {
                ModelState.AddModelError("", "Роль не найдена. Обратитесь к администратору");
                return View("Registration", model);
            }

            var profile = new Profile
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true,
                PhoneNumber = model.Phone,
                RoleId = role.Id
            };

            var registerResult = await _userManager.CreateAsync(profile, model.Password);
            if (registerResult.Succeeded)
            {
                var loginResult = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
                if (loginResult.Succeeded)
                    return RedirectToAction("Index");
            }

            foreach (var error in registerResult.Errors)
                ModelState.AddModelError("", error.Description);

            return View("Registration", model);
        }

        [HttpGet]
        [Route("login")]
        public IActionResult LoginPage()
        {
            return View("Login");
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel data)
        {
            if (!ModelState.IsValid)
                return View("Login", data);

            var result = await _signInManager.PasswordSignInAsync(data.Email, data.Password, data.RememberMe, false);
            if (result.Succeeded)
                return RedirectToAction("Index");

            ModelState.AddModelError("", "Неверный email или пароль");
            return View("Login", data);
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
