using Microsoft.AspNetCore.Mvc;

namespace MvcApp1.Controllers
{
    public class PageController : Controller
    {
        public IActionResult Welcome()
        {
            ViewBag.CurrentDate = DateTime.Now;
            return View();
        }

        public IActionResult Greet(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                name = "Гость";
            }

            ViewBag.UserName = name;
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            ViewBag.CurrentDate = DateTime.Now.ToString("dd.MM.yyyy");

            return View();
        }
    }
}