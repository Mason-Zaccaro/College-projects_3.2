using TextEditor.Models;

namespace TextEditor.Services
{
    public class TextStorageService
    {
        private readonly string _filePath;
        private readonly ILogger<TextStorageService> _logger;

        public TextStorageService(IWebHostEnvironment env, ILogger<TextStorageService> logger)
        {
            _filePath = Path.Combine(env.ContentRootPath, "Data", "saved_text.txt");
            _logger = logger;

            // Создаем папку Data если её нет
            var directory = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory!);
            }
        }

        public async Task<TextEditorModel> LoadTextAsync()
        {
            try
            {
                var model = new TextEditorModel();

                if (File.Exists(_filePath))
                {
                    model.Content = await File.ReadAllTextAsync(_filePath);
                    var fileInfo = new FileInfo(_filePath);
                    model.LastModified = fileInfo.LastWriteTime;
                }
                else
                {
                    model.Content = "Добро пожаловать в текстовый редактор!\n\nНачните печатать здесь...";
                    model.LastModified = DateTime.Now;
                }

                model.CharacterCount = model.Content.Length;
                model.WordCount = CountWords(model.Content);

                return model;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке текста");
                return new TextEditorModel
                {
                    Content = "Ошибка загрузки текста. Начните с нового документа.",
                    LastModified = DateTime.Now
                };
            }
        }

        public async Task<bool> SaveTextAsync(string content)
        {
            try
            {
                await File.WriteAllTextAsync(_filePath, content);
                _logger.LogInformation("Текст успешно сохранен");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении текста");
                return false;
            }
        }

        private int CountWords(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            return text.Split(new char[] { ' ', '\t', '\n', '\r' },
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }
    }
}