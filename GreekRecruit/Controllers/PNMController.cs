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

            return RedirectToAction("Index", new { id = pnm_id });
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Login");
        }
    }
}
