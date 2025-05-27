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

        public IActionResult Greet(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                id = "Гость";
            }

            ViewBag.UserName = id;
            ViewBag.CurrentTime = DateTime.Now.ToString("HH:mm:ss");
            ViewBag.CurrentDate = DateTime.Now.ToString("dd.MM.yyyy");

            return View();
        }

        // GET: /Page/Edit - отображает форму
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }

        // POST: /Page/Edit - обрабатывает отправленную форму
        [HttpPost]
        public IActionResult Edit(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                ViewBag.ErrorMessage = "Пожалуйста, введите сообщение!";
                return View();
            }

            // Передаем данные в представление
            ViewBag.UserMessage = message;
            ViewBag.SubmissionTime = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
            ViewBag.MessageLength = message.Length;
            ViewBag.IsSuccess = true;

            return View(); // Возвращаем то же представление, но с результатом
        }
    }
}