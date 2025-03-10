using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreekRecruit.Controllers
{
    public class RegisterController : Controller
    {
        private readonly SqlDataContext _context;

        public RegisterController(SqlDataContext context)
        {
            _context = context;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SubmitRegister(User model) 
        {
            String? uname = model.username;
            String? password1 = model.password;
            String? password2 = model.confirmPassword;

            if (uname.Length < 7 || uname.Length > 15)
            {
                TempData["FlashMessage"] = "Username must be between 8 and 15 characters!";
                return View("Register");
            }

            if (password1.Length < 7 || password1.Length > 20)
            {
                TempData["FlashMessage"] = "Password must be between 8 and 20 characters!";
                return View("Register");
            }
            if (password1 != password2)
            {
                TempData["FlashMessage"] = "Passwords must match!";
                return View("Register");
            }
            //if (password1)
            //WORK ON THIS
            return View();
        }
    }
}
