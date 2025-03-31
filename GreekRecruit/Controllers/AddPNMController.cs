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
    }
}
