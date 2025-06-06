using Microsoft.AspNetCore.Mvc;
using UserRegister.Models;
using UserRegister.Services;

namespace UserRegister.Controllers
{
    public class RegisterController : Controller
    {
        private readonly UserStorageService _storage;

        public RegisterController(UserStorageService storage)
        {
            _storage = storage;
        }

        [HttpGet]
        public IActionResult Index() => View();

        [HttpPost]
        public IActionResult Register(UserModel model)
        {
            if (ModelState.IsValid)
            {
                var userDto = new UserDto
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password
                };

                _storage.Add(userDto);
                ViewBag.Message = "Регистрация прошла успешно!";
                return View("Index");
            }

            return View("Index", model);
        }
    }
}
