using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using System.Text.Encodings.Web;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace GreekRecruit.Controllers;

public class HomeController : Controller
{

    private readonly SqlDataContext _context;

    public HomeController (SqlDataContext context)
    {
        _context = context;
    }

    //Homepage
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index(string? status)
    {
        var username = User.Identity?.Name;

        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

        if (user == null) return Unauthorized();


        var pnmsQuery = _context.PNMs.Where(p => p.organization_id == user.organization_id);

        var validStatuses = new[] { "Offered", "No Offer", "Pending", "Declined", "Accepted" };
        if (!string.IsNullOrEmpty(status) && validStatuses.Contains(status))
        {
            pnmsQuery = pnmsQuery.Where(p => p.pnm_status == status);
        }

        var pnms = await pnmsQuery.ToListAsync();
        return View(pnms);
    }

    //Logout
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login", "Login");
    }


}
