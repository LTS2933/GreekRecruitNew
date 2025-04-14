using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using GreekRecruit.Models;
using Microsoft.AspNetCore.Authentication;

namespace GreekRecruit.Controllers;

public class DashboardController : Controller
{
    private readonly SqlDataContext _context;

    public DashboardController(SqlDataContext context)
    {
        _context = context;
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Index()
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
        if (user == null) return Unauthorized();

        var orgId = user.organization_id;

        var totalPnms = await _context.PNMs.CountAsync(p => p.organization_id == orgId);
        var statusCounts = await _context.PNMs
            .Where(p => p.organization_id == orgId)
            .GroupBy(p => p.pnm_status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync();

        var statusDict = statusCounts.ToDictionary(k => k.Status ?? "Unknown", v => v.Count);

        var averageGpa = await _context.PNMs
            .Where(p => p.organization_id == orgId && p.pnm_gpa.HasValue)
            .AverageAsync(p => p.pnm_gpa.Value);

        var totalEvents = await _context.Events
            .CountAsync(e => e.organization_id == orgId);

        var mostRecentVoted = await _context.PNMVoteSessions
            .Where(v => _context.PNMs.Any(p => p.pnm_id == v.pnm_id && p.organization_id == orgId))
            .OrderByDescending(v => v.session_open_dt)
            .Take(5)
            .Select(v => new
            {
                Session = v,
                PNM = _context.PNMs.FirstOrDefault(p => p.pnm_id == v.pnm_id)
            })
            .ToListAsync();


        ViewData["StatusCounts"] = statusDict;
        ViewData["TotalPNMs"] = totalPnms;
        ViewData["AvgGPA"] = averageGpa;
        ViewData["TotalEvents"] = totalEvents;
        ViewData["RecentVotes"] = mostRecentVoted;

        return View();
    }
    //Logout
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login", "Login");
    }
}
