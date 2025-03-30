using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;

namespace GreekRecruit.Controllers
{
    public class AddPNMController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SubmitPNM(PNM pnm)
        {
            var fname = pnm.pnm_fname;
            var lname = pnm.pnm_lname;
            if (fname == null || lname == null)
            {
                ViewData["FlashMessage"] = "PNM's name cannot be empty!";
                return View("Index");
            }

            var email = pnm.pnm_email;
            var phone = pnm.pnm_phone;
            var gpa = pnm.pnm_gpa;
            var major = pnm.pnm_major;
            var schoolyear = pnm.pnm_schoolyear;

            //NEED TO FINISH THIS
            return View("Index");

        }
    }
}
