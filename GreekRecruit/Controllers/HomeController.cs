using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using System.Text.Encodings.Web;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace GreekRecruit.Controllers;

public class HomeController : Controller
{

    private readonly SqlDataContext _context;

    public HomeController (SqlDataContext context)
    {
        _context = context;
    }

    [Authorize]
    public IActionResult Index()
    {
        var username = User.Identity?.Name;

        var user = _context.Users.FirstOrDefault(u => u.username == username);

        if (user == null)
        {
            return Unauthorized(); 
        }

        var pnms = _context.PNMs
                          .Where(p => p.organization_id == user.organization_id)
                          .ToList();

        return View(pnms);
    }


    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login", "Login");
    }


}
