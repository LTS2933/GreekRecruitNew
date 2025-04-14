using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace GreekRecruit.Controllers;

public class InterestFormController : Controller
{
    private readonly SqlDataContext _context;

    public InterestFormController(SqlDataContext context)
    {
        _context = context;
    }

    // View all Interest Forms (Admin)
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

        if (user == null ) return Unauthorized();

        var forms = await _context.InterestForms
            .Where(f => f.organization_id == user.organization_id)
            .OrderByDescending(f => f.date_created)
            .ToListAsync();

        return View(forms);
    }

    // The view for creating a new Interest Form (Admin)
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Create()
    {

        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

        if (user == null) return Unauthorized();
        if (user.role != "Admin") return Forbid();

        return View();

    }

    // Handle form submission for creating a new Interest Form
    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(string form_name)
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

        if (user == null) return Unauthorized();
        if (user.role != "Admin") return Forbid();

        if (string.IsNullOrWhiteSpace(form_name))
        {
            TempData["ErrorMessage"] = "Form name cannot be empty.";
            return RedirectToAction("Create");
        }

        var form = new InterestForm
        {
            form_name = form_name.Trim(),
            organization_id = user.organization_id
        };

        _context.InterestForms.Add(form);
        await _context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Form '{form.form_name}' created!";
        return RedirectToAction("Index");
    }

    // View submissions for a specific form (Admin)
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Submissions(int form_id)
    {
        var username = User.Identity?.Name;
        var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

        if (user == null) return Unauthorized();

        var form = await _context.InterestForms
            .FirstOrDefaultAsync(f => f.form_id == form_id && f.organization_id == user.organization_id);

        if (form == null)
            return NotFound();

        var submissions = await _context.InterestFormSubmissions
            .Where(s => s.form_id == form_id)
            .OrderByDescending(s => s.date_submitted)
            .ToListAsync();

        ViewData["FormName"] = form.form_name;
        return View(submissions);
    }

    // Public Submission View (Accessible to anyone)
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> SubmitView(int form_id)
    {
        var form = await _context.InterestForms.FirstOrDefaultAsync(f => f.form_id == form_id);
        if (form == null)
            return NotFound("Form does not exist.");

        ViewData["FormName"] = form.form_name;
        ViewData["FormId"] = form_id;
        return View();
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SubmitForm(int form_id, InterestFormSubmission submission, IFormFile? pnm_profilepicture)
    {
        var form = await _context.InterestForms.FirstOrDefaultAsync(f => f.form_id == form_id);
        if (form == null)
            return NotFound("Form does not exist.");

        if (!ModelState.IsValid)
        {
            ViewData["FormName"] = form.form_name;
            ViewData["FormId"] = form_id;
            TempData["ErrorMessage"] = "Please fill out all required fields.";
            return View(submission);
        }

        if (pnm_profilepicture != null && pnm_profilepicture.Length > 0)
        {
            using var ms = new MemoryStream();
            await pnm_profilepicture.CopyToAsync(ms);
            submission.pnm_profilepicture = ms.ToArray();
        }

        submission.form_id = form_id;
        submission.organization_id = form.organization_id;
        submission.date_submitted = DateTime.Now;

        _context.InterestFormSubmissions.Add(submission);

        // Add to PNMs table
        var pnm = new PNM
        {
            organization_id = form.organization_id,
            pnm_fname = submission.pnm_fname,
            pnm_lname = submission.pnm_lname,
            pnm_email = submission.pnm_email,
            pnm_phone = submission.pnm_phone,
            pnm_schoolyear = submission.pnm_schoolyear,
            pnm_major = submission.pnm_major,
            pnm_gpa = submission.pnm_gpa,
            pnm_profilepicture = submission.pnm_profilepicture,
            pnm_instagramhandle = submission.pnm_instagramhandle
        };

        _context.PNMs.Add(pnm);
        await _context.SaveChangesAsync();

        return RedirectToAction("ThankYou");
    }


    [AllowAnonymous]
    [HttpGet]
    public IActionResult ThankYou()
    {
        return View();
    }
}


//LOOK BACK AT THIS CONTROLLER AND RENAME METHODS