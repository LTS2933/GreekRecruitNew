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
    public async Task<IActionResult> Index(string? status, string? search, string? sort)
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

        if (user == null) return Unauthorized();

        var pnmsQuery = _context.PNMs
            .Where(p => p.organization_id == user.organization_id);

        var validStatuses = new[] { "Offered", "No Offer", "Pending", "Declined", "Accepted" };
        if (!string.IsNullOrEmpty(status) && validStatuses.Contains(status))
        {
            pnmsQuery = pnmsQuery.Where(p => p.pnm_status == status);
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.ToLower();
            pnmsQuery = pnmsQuery.Where(p =>
                (p.pnm_fname + " " + p.pnm_lname).ToLower().Contains(search));
        }

        pnmsQuery = sort switch
        {
            "name_asc" => pnmsQuery.OrderBy(p => p.pnm_lname).ThenBy(p => p.pnm_fname),
            "name_desc" => pnmsQuery.OrderByDescending(p => p.pnm_lname).ThenByDescending(p => p.pnm_fname),
            "gpa_asc" => pnmsQuery.OrderBy(p => p.pnm_gpa),
            "gpa_desc" => pnmsQuery.OrderByDescending(p => p.pnm_gpa),
            _ => pnmsQuery.OrderBy(p => p.pnm_lname)
        };

        var pnms = await pnmsQuery.ToListAsync();

        List<AdminTask> taskPreview = new();
        if (user.role == "Admin")
        {
            taskPreview = await _context.AdminTasks
                .Where(t => t.organization_id == user.organization_id && !t.is_completed)
                .OrderBy(t => t.due_date ?? t.date_created)
                .Take(3)
                .ToListAsync();
        }

        ViewData["TaskPreview"] = taskPreview;

        return View(pnms);
    }


    //Applies a status change or deletes PNMs from homepage
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BatchUpdate(int[] selectedPnms, string newStatus, bool delete = false)
    {
        var username = User.Identity.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);
        if (user == null) return Unauthorized();
        if (user.role != "Admin") return Forbid();

        if (selectedPnms == null || selectedPnms.Length == 0)
        {
            TempData["ErrorMessage"] = "No PNMs selected.";
            return RedirectToAction("Index");
        }

        var pnms = await _context.PNMs.Where(p => selectedPnms.Contains(p.pnm_id)).ToListAsync();

        if (delete)
        {
            _context.PNMs.RemoveRange(pnms);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"{pnms.Count} PNMs deleted.";
            return RedirectToAction("Index");
        }

        if (!string.IsNullOrEmpty(newStatus))
        {
            pnms.ForEach(p => p.pnm_status = newStatus);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Status updated successfully.";
        }
        else
        {
            TempData["ErrorMessage"] = "No status selected.";
        }

        return RedirectToAction("Index");
    }


    //Logout
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync("MyCookieAuth");
        return RedirectToAction("Login", "Login");
    }


}
