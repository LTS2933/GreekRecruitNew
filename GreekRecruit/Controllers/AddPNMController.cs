using GreekRecruit.Models;
using Microsoft.AspNetCore.Authentication;
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
        [HttpGet]
        [Authorize]
        
        //Returns the view to Add a PNM
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [Authorize]
        //Returns the view for batch adding PNMs via a CSV file
        public IActionResult AddPNMCSV()
        {
            return View("AddPNMCSV");
        }

        //Submits a new PNM with all datapoints from the form within the view
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SubmitPNM(PNM pnm, IFormFile uploadedProfilePicture)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

            if (user == null)
            {
                ViewData["ErrorMessage"] = "User not found. Please log in again.";
                return Unauthorized();
            }

            if (string.IsNullOrWhiteSpace(pnm.pnm_fname) || string.IsNullOrWhiteSpace(pnm.pnm_lname))
            {
                ViewData["ErrorMessage"] = "PNM's name cannot be empty!";
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

                pnm.organization_id = user.organization_id;
                _context.PNMs.Add(pnm);
                await _context.SaveChangesAsync();

                ViewData["SuccessMessage"] = "PNM submitted successfully!";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ViewData["ErrorMessage"] = "Something went wrong while submitting the form. Please try again.";
            }

            return RedirectToAction("Index", "Home");


        }

        //Batch Add PNMs from a CSV file
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportCSV(IFormFile csvFile)
        {
            var username = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.username == username);

            if (user == null) return Unauthorized();

            if (user.role != "Admin") return Forbid();

            if (csvFile == null || csvFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Please upload a valid CSV file.";
                return RedirectToAction("AddPNMCSV");
            }

            var expectedHeaders = new[]
            {
                "pnm_fname",
                "pnm_lname",
                "pnm_email",
                "pnm_phone",
                "pnm_gpa",
                "pnm_major",
                "pnm_schoolyear",
                "pnm_instagramhandle"
            };

            using var stream = new StreamReader(csvFile.OpenReadStream());
            int lineNum = 0;
            var newPnms = new List<PNM>();
            var skippedRows = new List<int>();

            string? headerLine = await stream.ReadLineAsync();
            lineNum++;

            if (headerLine == null)
            {
                TempData["ErrorMessage"] = "CSV file is empty. Please provide data.";
                return RedirectToAction("AddPNMCSV");
            }

            var headers = headerLine.Split(',')
                                    .Select(h => h.Trim().ToLower())
                                    .ToArray();

            if (headers.Length != expectedHeaders.Length ||
                !headers.SequenceEqual(expectedHeaders))
            {
                TempData["ErrorMessage"] = "CSV header is invalid. Please use the exact column names and order.";
                return RedirectToAction("AddPNMCSV");
            }

            while (!stream.EndOfStream)
            {
                var line = await stream.ReadLineAsync();
                lineNum++;

                if (line == null)
                    continue;

                var rawFields = line.Split(',');

                var safeFields = new string[8];
                for (int i = 0; i < rawFields.Length && i < 8; i++)
                {
                    safeFields[i] = rawFields[i]?.Trim();
                }

                var fName = safeFields[0];
                var lName = safeFields[1];
                var email = safeFields[2];
                var phone = safeFields[3];
                var gpaField = safeFields[4];
                var major = safeFields[5];
                var schoolYear = safeFields[6];
                var insta = safeFields[7];

                if (string.IsNullOrWhiteSpace(fName) && string.IsNullOrWhiteSpace(lName))
                {
                    skippedRows.Add(lineNum);
                    continue;
                }

                double? gpaValue = null;
                if (double.TryParse(gpaField, out double parsedGpa))
                {
                    gpaValue = parsedGpa;
                }

                var pnm = new PNM
                {
                    organization_id = user.organization_id,
                    pnm_fname = fName,
                    pnm_lname = lName,
                    pnm_email = email,
                    pnm_phone = phone,
                    pnm_gpa = gpaValue,
                    pnm_major = major,
                    pnm_schoolyear = schoolYear,
                    pnm_instagramhandle = insta
                };

                newPnms.Add(pnm);
            }

            _context.PNMs.AddRange(newPnms);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = $"{newPnms.Count} PNMs successfully imported. " +
                (skippedRows.Any() ? $"Skipped rows: {string.Join(", ", skippedRows)}." : "");

            return RedirectToAction("Index", "Home");
        }

        //Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Login");
        }
    }
}
