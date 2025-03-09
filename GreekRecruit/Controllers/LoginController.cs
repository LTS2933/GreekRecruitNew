using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;

namespace GreekRecruit.Controllers
{
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
            public IActionResult SubmitData(User model)
        {
            var uname = model.username;
            bool exists = _context.Users.Any(u => u.username == uname);
            if (!exists)
            {
                TempData["FlashMessage"] = "Username does not exist!";
                return View("Login");
            }
            return View("~/Views/Home/Index.cshtml");

        }
    }
}
