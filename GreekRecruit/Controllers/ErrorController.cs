using Microsoft.AspNetCore.Mvc;

public class ErrorController : Controller
{
    [Route("Error/{statusCode}")]
    public IActionResult HandleError(int statusCode)
    {
       // if (statusCode == 404)
       // {
            return View("NotFound");
        //}

        //return View("GenericError"); // Fallback
    }
}

