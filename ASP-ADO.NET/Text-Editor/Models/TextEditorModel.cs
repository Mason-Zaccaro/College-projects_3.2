namespace TextEditor.Models
{
    public class TextEditorModel
    {
        public string Content { get; set; } = string.Empty;
        public DateTime LastModified { get; set; }
        public int WordCount { get; set; }
        public int CharacterCount { get; set; }
        public string FileName { get; set; } = "document.txt";
    }
}