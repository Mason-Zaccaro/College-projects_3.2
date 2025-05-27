using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TextEditor.Models;
using TextEditor.Services;

namespace TextEditor.Pages
{
    public class EditorModel : PageModel
    {
        private readonly TextStorageService _textStorage;
        private readonly ILogger<EditorModel> _logger;

        public EditorModel(TextStorageService textStorage, ILogger<EditorModel> logger)
        {
            _textStorage = textStorage;
            _logger = logger;
        }

        [BindProperty]
        public string TextContent { get; set; } = string.Empty;

        public TextEditorModel EditorData { get; set; } = new();
        public bool SaveSuccess { get; set; }
        public string SaveMessage { get; set; } = string.Empty;

        public async Task OnGetAsync()
        {
            EditorData = await _textStorage.LoadTextAsync();
            TextContent = EditorData.Content;
        }

        public async Task<IActionResult> OnPostSaveAsync()
        {
            if (string.IsNullOrEmpty(TextContent))
            {
                SaveMessage = "Нельзя сохранить пустой документ!";
                SaveSuccess = false;
            }
            else
            {
                SaveSuccess = await _textStorage.SaveTextAsync(TextContent);
                SaveMessage = SaveSuccess ?
                    $"Документ успешно сохранен в {DateTime.Now:HH:mm:ss}" :
                    "Ошибка при сохранении документа";
            }

            // Обновляем данные после сохранения
            EditorData = await _textStorage.LoadTextAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostClearAsync()
        {
            TextContent = "";
            EditorData = new TextEditorModel
            {
                Content = "",
                LastModified = DateTime.Now
            };

            SaveMessage = "Документ очищен";
            SaveSuccess = true;

            return Page();
        }
    }
}