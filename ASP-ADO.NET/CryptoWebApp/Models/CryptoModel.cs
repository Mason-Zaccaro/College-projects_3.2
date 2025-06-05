namespace CryptoWebApp.Models
{
    public class CryptoModel
    {
        public string Text { get; set; } = "";
        public string Algorithm { get; set; } = "AES";
        public string Key { get; set; } = "";
        public string EncryptedText { get; set; } = "";
        public string DecryptedText { get; set; } = "";
    }
}
