using GreekRecruit.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;

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

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("MyCookieAuth");
            return RedirectToAction("Login", "Login");
        }

        [HttpPost]
        public IActionResult AddUserData(string email)
        {
            if (_context.Users.Any(u => u.email == email))
            {
                TempData["ErrorMessage"] = $"Email {email} already exists!";
                return RedirectToAction("AddUsers");
            }
            else
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


                    int ampersand_index = email.IndexOf("@");
                    if (ampersand_index > 0)
                    {
                        User user = new User();
                        user.username = email.Substring(0, ampersand_index);
                        user.email = email;
                        user.role = "User";
                        user.password = GenerateRandomPassword();

                        var current_user_username = User.Identity?.Name;
                        var current_user = _context.Users.FirstOrDefault(u => u.username == current_user_username);

                        if (current_user == null)
                        {
                            ViewData["FlashMessage"] = "User not found. Please log in again.";
                            return RedirectToAction("Login", "Login");
                        }

                        user.organization_id = current_user.organization_id;

                        _context.Add<User>(user);
                        _context.SaveChanges();

                        mail.Subject = "Join GreekRecruit!";
                        mail.Body = $"You've been invited to join GreekRecruit by your admin, {User.Identity.Name}.\nYou can now log in using this email.\nUsername: {user.username}\nPassword: {user.password}" +
                        "\nIf you would like to rest your password, you can do so by clicking the Settings button in your profile dropdown.";

                        var smtpClient = new SmtpClient(smtpServer)
                        {
                            Port = port,
                            Credentials = new NetworkCredential(username, password),
                            EnableSsl = true,
                        };

                        smtpClient.Send(mail);
                        TempData["SuccessMessage"] = $"Email sent to {email}";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Invalid email address! Please input a valid email address.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = $"Error sending email: {ex.Message}";
                }

                return RedirectToAction("AddUsers");
            }
        }
        private string GenerateRandomPassword()
        {
            const string alphanumerics = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            const string specialChars = "!@#$%^&*()-_=+<>?";

            var random = new Random();
            var passwordChars = new char[8];

            for (int i = 0; i < 6; i++)
                passwordChars[i] = alphanumerics[random.Next(alphanumerics.Length)];

            for (int i = 6; i < 8; i++)
                passwordChars[i] = specialChars[random.Next(specialChars.Length)];

            return new string(passwordChars.OrderBy(c => Guid.NewGuid()).ToArray());
        }

    }
}
