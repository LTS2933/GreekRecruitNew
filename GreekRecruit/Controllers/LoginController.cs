using GreekRecruit.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GreekRecruit.Controllers
{
    //[Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class LoginController : Controller
    {

        private readonly SqlDataContext _context;

        public LoginController(SqlDataContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
            public async Task<IActionResult> SubmitDataAsync(User model)
        {
            var uname = model.username;
            bool exists = _context.Users.Any(u => u.username == uname);
            if (!exists)
            {
                TempData["FlashMessage"] = "Username does not exist!";
                return View("Login");
            }
            var pword = model.password;
            bool correctUser = _context.Users.Any(u => u.username == uname && u.password == pword);
            if (!correctUser) {
                TempData["FlashMessage"] = "Incorrect Password!";
                return View("Login");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, uname)
            };

            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("MyCookieAuth", principal);

            return RedirectToAction("Index", "Home");


        }
    }
}
