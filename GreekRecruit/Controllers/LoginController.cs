using GreekRecruit.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        //Login page
        public IActionResult Login()
        {
            return View();
        }

        //Submit user credentials to try to log in
        [HttpPost]
        public async Task<IActionResult> SubmitDataAsync(User model)
        {
            var email = model.email;
            //bool emailExists = _context.Users.Any(u => u.email == email);
            //if (!emailExists)
            //{
            //    TempData["FlashMessage"] = "Email does not exist within this organization's users!";
            //    return View("Login");
            //}

            var uname = model.username;
            //bool usernameExists = _context.Users.Any(u => u.username == uname);
            //if (!usernameExists)
            //{
            //    TempData["FlashMessage"] = "Username does not exist within this organization's users!";
            //    return View("Login");
            //}
            var pword = model.password;
            bool correctUser = await _context.Users.AnyAsync(u => u.username == uname && u.password == pword && u.email == email);
            if (!correctUser) {
                TempData["ErrorMessage"] = "Invalid credentials!";
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
