using GreekRecruit.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GreekRecruit.Controllers
{
    public class AddPNMController : Controller
    {
        private readonly SqlDataContext _context;

        public AddPNMController(SqlDataContext context)
        {
            _context = context;
        }

        [Authorize]
        
        //Returns the view to Add a PNM
        public IActionResult Index()
        {
            return View();
        }

        //Submits a new PNM with all datapoints from the form within the view
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitPNM(PNM pnm, IFormFile uploadedProfilePicture)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

            if (user == null)
            {
                ViewData["FlashMessage"] = "User not found. Please log in again.";
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(pnm.pnm_fname) || string.IsNullOrWhiteSpace(pnm.pnm_lname))
            {
                ViewData["FlashMessage"] = "PNM's name cannot be empty!";
                return View("Index");
            }

            try
            {
                if (uploadedProfilePicture != null && uploadedProfilePicture.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await uploadedProfilePicture.CopyToAsync(ms);
                        pnm.pnm_profilepicture = ms.ToArray();
                    }
                }

                pnm.organization_id = user.organization_id;
                _context.PNMs.Add(pnm);
                await _context.SaveChangesAsync();

                ViewData["FlashMessage"] = "PNM submitted successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ViewData["FlashMessage"] = "Something went wrong while submitting the form. Please try again.";
            }

            return RedirectToAction("Index", "Home");


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
