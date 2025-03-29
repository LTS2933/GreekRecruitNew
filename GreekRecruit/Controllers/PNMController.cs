using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace GreekRecruit.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class PNMController : Controller
    {

        private readonly SqlDataContext _context;

        public PNMController(SqlDataContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var pnm = _context.PNMs.FirstOrDefault(p => p.pnm_id == id);
            if (pnm == null) return NotFound();

            var comments = _context.Comments
                .Where(c => c.pnm_id == id)
                .OrderByDescending(c => c.comment_dt)
                .ToList();
            if (comments == null) return NotFound();

            return View((pnm, comments));
        }


        [HttpPost("PNM/SubmitComment/{pnm_id}")]
        public IActionResult SubmitComment(Comment comment, int pnm_id)
        {
            DateTime comment_dt = DateTime.Now;

            string comment_text = comment.comment_text;
            comment.comment_dt = comment_dt;
            comment.pnm_id = pnm_id;
            comment.comment_author = User.Identity.Name;

            _context.Add<Comment>(comment);
            _context.SaveChanges();

            TempData["FlashMessage"] = "Changes Saved.";

            return RedirectToAction("Index", new { id = pnm_id });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Login");
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(IFormCollection form)
        {
            string? pnm_status = form["pnm_status"];
            var pnm_id_string = form["pnm_id"];

            if (!int.TryParse(pnm_id_string, out int pnm_id))
            {
                return BadRequest("Invalid PNM ID.");
            }

            var pnm = await _context.PNMs.FindAsync(pnm_id);
            if (pnm == null)
            {
                return NotFound();
            }

            pnm.pnm_status = pnm_status;
            await _context.SaveChangesAsync();

            TempData["FlashMessage"] = "Changes saved.";

            return RedirectToAction("Index", new { id = pnm_id });
        }


    }
}
