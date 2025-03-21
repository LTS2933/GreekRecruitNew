using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using System.Security.Claims;

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

        public IActionResult Index()
        {
            return View();
        }
    }
}
