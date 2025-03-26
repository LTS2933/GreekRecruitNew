using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace GreekRecruit.Controllers
{
    [Authorize(AuthenticationSchemes = "MyCookieAuth")]
    public class ProfileController : Controller
    {
        private readonly SqlDataContext _context;
        private readonly IConfiguration _configuration;

        public ProfileController(SqlDataContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.username == username);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        public IActionResult AddUsers()
        {
            var username = User.Identity?.Name;
            var user = _context.Users.FirstOrDefault(u => u.username == username);
            if (user?.role == "Admin")
            {
                return View();
            }
            else
            {
                return Forbid();
            }
        }

        [HttpPost]
        public IActionResult AddUserData(string email)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                var smtpServer = emailSettings["SmtpServer"];
                var port = int.Parse(emailSettings["Port"]);
                var username = emailSettings["Username"];
                var password = emailSettings["Password"];

                var mail = new MailMessage();
                mail.From = new MailAddress(username);
                mail.To.Add(email);
                mail.Subject = "Join GreekRecruit!";
                mail.Body = "You've been invited to join GreekRecruit by " + User.Identity.Name;

                var smtpClient = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Credentials = new NetworkCredential(username, password),
                    EnableSsl = true,
                };

                smtpClient.Send(mail);
                TempData["SuccessMessage"] = $"Email sent to {email}";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error sending email: {ex.Message}";
            }

            return RedirectToAction("AddUsers");
        }
    }
}
