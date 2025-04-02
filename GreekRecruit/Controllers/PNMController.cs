using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

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

        [Authorize]
        public IActionResult Index(int id)
        {
            var pnm = _context.PNMs.FirstOrDefault(p => p.pnm_id == id);
            if (pnm == null)
            {
                TempData["FlashMessage"] = "PNM ID not found.";
                return RedirectToAction("Index", "Home");
            }

            var comments = _context.Comments
                .Where(c => c.pnm_id == id)
                .OrderByDescending(c => c.comment_dt)
                .ToList();
            if (comments == null) return NotFound();

            return View((pnm, comments));
        }

        [Authorize]
        [HttpPost("PNM/SubmitComment/{pnm_id}")]
        public async Task<IActionResult> SubmitComment(Comment comment, int pnm_id)
        {
            try
            {

                var username = User.Identity?.Name;
                var user = _context.Users.FirstOrDefault(u => u.username == username);

                if (user == null)
                {
                    return Unauthorized();
                }

                comment.comment_dt = DateTime.Now;
                comment.pnm_id = pnm_id;
                comment.comment_author = username ?? "Unknown";
                comment.comment_author_name = user.full_name ?? "Unknown";

                if (string.IsNullOrEmpty(comment.comment_text))
                {
                    TempData["FlashMessage"] = "Comment cannot be empty. Please try again.";

                    return RedirectToAction("Index", new { id = pnm_id });
                }

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();

                TempData["FlashMessage"] = "Changes Saved.";
                return RedirectToAction("Index", new { id = pnm_id });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                TempData["FlashMessage"] = "Something went wrong while submitting the comment. Please try again.";

                return RedirectToAction("Index", new { id = pnm_id });
            }
        }


        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditStatus(IFormCollection form)
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

            try
            {
                pnm.pnm_status = pnm_status;
                await _context.SaveChangesAsync();

                TempData["FlashMessage"] = "Changes saved.";

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["FlashMessage"] = "Error saving changes.";
            }
            return RedirectToAction("Index", new { id = pnm_id });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EditInfo(IFormCollection form)
        {
            if (!int.TryParse(form["pnm_id"], out int pnm_id))
            {
                return BadRequest("Invalid PNM ID.");
            }

            var pnm = await _context.PNMs.FindAsync(pnm_id);
            if (pnm == null)
            {
                return NotFound();
            }

            try
            {
                pnm.pnm_email = form["pnm_email"];
                pnm.pnm_phone = form["pnm_phone"];
                if (double.TryParse(form["pnm_gpa"], out double parsedGpa))
                {
                    pnm.pnm_gpa = parsedGpa;
                }
                else
                {
                    TempData["FlashMessage"] = "Invalid GPA format. Please enter a valid number.";
                    return RedirectToAction("Index", new { id = pnm.pnm_id });
                }

                pnm.pnm_major = form["pnm_major"];
                pnm.pnm_schoolyear = form["pnm_schoolyear"];

                await _context.SaveChangesAsync();
                TempData["FlashMessage"] = "PNM info updated successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["FlashMessage"] = "Error updating PNM info.";
            }

            return RedirectToAction("Index", new { id = pnm_id });
        }

        [HttpPost]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateProfilePicture(IFormFile newProfilePicture, int pnm_id)
        {
            if (newProfilePicture == null || newProfilePicture.Length == 0)
            {
                TempData["FlashMessage"] = "Please select a valid image.";
                return RedirectToAction("Index", new { id = pnm_id });
            }

            var pnm = await _context.PNMs.FindAsync(pnm_id);
            if (pnm == null)
            {
                return NotFound();
            }

            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    await newProfilePicture.CopyToAsync(memoryStream);
                    pnm.pnm_profilepicture = memoryStream.ToArray();
                }

                await _context.SaveChangesAsync();
                TempData["FlashMessage"] = "Profile picture updated successfully.";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                TempData["FlashMessage"] = "Error updating profile picture.";
            }

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
