using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using System.Security.Claims;




namespace GreekRecruit.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class ProfileController : Controller
    {

        private readonly SqlDataContext _context;

        public ProfileController(SqlDataContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.username == username);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult AddData()
        {
            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.username == username);
            if (user?.role == "Admin")
            {
                return View();
                //Need to work on view, AddData.cshtml under Profile folder
            }
            else
            {
                return Forbid();
            }
        }

    }
}
