using Microsoft.AspNetCore.Mvc;

namespace CryptoWebApp.Models
{
    public class CryptoViewModel
    {
        // Текст, введенный пользователем
        public string InputText { get; set; }

        // Выбранный алгоритм: "AES" или "RSA"
        public string Algorithm { get; set; }

        // Текстовое представление ключа (для AES — base64 строки ключа и IV, для RSA — XML или XML-строка)
        public string Key { get; set; }

        // Текст после шифрования (Base64) или после дешифрования
        public string ResultText { get; set; }

        // Флаг: нужно ли сгенерировать новый ключ автоматически (true/false)
        public bool GenerateKey { get; set; }
    }
}

