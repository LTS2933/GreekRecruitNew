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
        var pnms = _context.PNMs.ToList();
        return View(pnms);
    }
    public String Welcome(String name, int numTimes=2)
    {
        return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login", "Login");
    }


}
