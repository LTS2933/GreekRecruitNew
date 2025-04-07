using GreekRecruit.Models;
using Microsoft.AspNetCore.Authentication;
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

        [AllowAnonymous]
        public async Task<IActionResult> Index(int event_id)
        {
            var r_event = await _context.Events.FirstOrDefaultAsync(e => e.event_id == event_id);
            if (r_event == null)
            {
                TempData["FlashMessage"] = "Event ID not found.";
                return RedirectToAction("Index", "Home");
            }

            return View(r_event);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SubmitAttendance(EventAttendance attendance)
        {
            if (!ModelState.IsValid)
            {
                TempData["FlashMessage"] = "Please fill out all required fields.";
                return RedirectToAction("Index", new { event_id = attendance.event_id });
            }

            if (string.IsNullOrWhiteSpace(attendance.pnm_fname) || string.IsNullOrWhiteSpace(attendance.pnm_lname))
            {
                TempData["FlashMessage"] = "Name is required.";
                return RedirectToAction("Index", new { event_id = attendance.event_id });
            }

            attendance.checked_in_at = DateTime.Now;

            _context.EventsAttendance.Add(attendance);
            await _context.SaveChangesAsync();

            TempData["FlashMessage"] = "Thanks! Your attendance has been recorded.";
            return RedirectToAction("ThankYou");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ThankYou()
        {
            return View();
        }
        //Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Login");
        }

    }
}
