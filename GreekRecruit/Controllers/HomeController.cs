using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GreekRecruit.Models;
using System.Text.Encodings.Web;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;

namespace GreekRecruit.Controllers;

public class HomeController : Controller
{
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }
    public String Welcome(String name, int numTimes=2)
    {
        return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
    }

    
}
