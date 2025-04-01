using GreekRecruit.Models;
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
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitPNM(PNM pnm, IFormFile uploadedProfilePicture)
        {
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
                var username = User.Identity?.Name;
                var user = _context.Users.FirstOrDefault(u => u.username == username);

                if (user == null)
                {
                    ViewData["FlashMessage"] = "User not found. Please log in again.";
                    return RedirectToAction("Login", "Login");
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


            //Work on getting the "L" out of all dropdowns and instead grabbing the first letter from username and uppercasing it

        }
    }
}
