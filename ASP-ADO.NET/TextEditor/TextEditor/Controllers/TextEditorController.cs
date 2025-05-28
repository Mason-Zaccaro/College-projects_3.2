using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text;

namespace TextEditor.Controllers
{
    public class TextEditorController : Controller
    {
        private readonly string _filePath;

        public TextEditorController(IWebHostEnvironment environment)
        {
            _filePath = Path.Combine(environment.ContentRootPath, "App_Data", "saved_text.txt");

            // Создаем директорию если она не существует
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var model = new TextEditorViewModel();

            // Загружаем сохраненный текст, если файл существует
            if (System.IO.File.Exists(_filePath))
            {
                try
                {
                    model.Content = await System.IO.File.ReadAllTextAsync(_filePath, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Ошибка при загрузке текста: {ex.Message}";
                    model.Content = "";
                }
            }
            else
            {
                model.Content = "";
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveText(string content)
        {
            try
            {
                await System.IO.File.WriteAllTextAsync(_filePath, content ?? "", Encoding.UTF8);
                ViewBag.Message = "Текст успешно сохранен";
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ошибка при сохранении: {ex.Message}";
            }

            // Загружаем сохраненный текст для отображения
            var model = new TextEditorViewModel();
            if (System.IO.File.Exists(_filePath))
            {
                model.Content = await System.IO.File.ReadAllTextAsync(_filePath, Encoding.UTF8);
                model.SavedContent = model.Content;
            }
            else
            {
                model.Content = content ?? "";
                model.SavedContent = "";
            }

            return View("Index", model);
        }

        [HttpPost]
        public async Task<IActionResult> LoadText()
        {
            var model = new TextEditorViewModel();

            if (System.IO.File.Exists(_filePath))
            {
                try
                {
                    var savedContent = await System.IO.File.ReadAllTextAsync(_filePath, Encoding.UTF8);
                    model.Content = savedContent;
                    model.SavedContent = savedContent;
                    ViewBag.Message = "Текст успешно загружен";
                }
                catch (Exception ex)
                {
                    ViewBag.ErrorMessage = $"Ошибка при загрузке: {ex.Message}";
                    model.Content = "";
                    model.SavedContent = "";
                }
            }
            else
            {
                ViewBag.ErrorMessage = "Сохраненный файл не найден";
                model.Content = "";
                model.SavedContent = "";
            }

            return View("Index", model);
        }
    }

    public class TextEditorViewModel
    {
        public string Content { get; set; } = "";
        public string SavedContent { get; set; } = "";
    }
}