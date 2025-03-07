using Microsoft.AspNetCore.Mvc;

namespace GreekRecruit.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
            public IActionResult SubmitData()
        {
            return View("~/Views/Home/Privacy.cshtml");
        }
    }
}
