using AuthenticationSample.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace AuthenticationSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AuthenticationService _authenticationService;

        public HomeController(ILogger<HomeController> logger, AuthenticationService authenticationService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
        }

        public IActionResult Index(bool errorLogin)
        {
            if (errorLogin)
            {
                ViewBag.Error = "username or password is invalid";
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            var userDbmock = new User
            {
                UserName = "Teste",
                Password = "123456",
                Position = "Admin"
            };

            if (user.UserName != userDbmock.UserName ||
                user.Password != userDbmock.Password)
            {
                return RedirectToAction("Index", new { errorLogin = true });

            }

            await _authenticationService.Login(HttpContext, userDbmock);
            return RedirectToAction("Privacy");

        }

        public async Task<IActionResult> Logout()
        {
            await _authenticationService.Logout(HttpContext);
            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult Privacy()
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
