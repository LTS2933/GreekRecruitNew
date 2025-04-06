using GreekRecruit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreekRecruit.Controllers
{
    public class EventAttendanceController : Controller
    {
        private readonly SqlDataContext _context;
        public EventAttendanceController (SqlDataContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            if (user == null) return Unauthorized();
            
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitAttendance(EventAttendance attendance)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
            if (user == null) return Unauthorized();

            return View(); //Gotta fix this
        }
    }
}
