using Microsoft.AspNetCore.Mvc;

namespace TraffiLearn.WebApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("/")]
        public IActionResult MainPage()
        {
            return View();
        }
    }
}
