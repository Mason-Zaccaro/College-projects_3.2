using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AdminPanel.Controllers
{
    public class RegisterController : Controller
    {
        // In-memory список пользователей
        private static List<User> _users = new List<User>();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(User model)
        {
            if (ModelState.IsValid)
            {
                _users.Add(model); // Регистрация (имитация добавления)
                ViewBag.Message = "Регистрация прошла успешно!";
                ModelState.Clear();
                return View();
            }

            return View(model);
        }
    }
}
