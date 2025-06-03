using Microsoft.AspNetCore.Mvc;
using CryptoWebApp.Models;
using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptoWebApp.Controllers
{
    public class CryptoController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            // При первом заходе форма пуста
            return View(new CryptoViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(CryptoViewModel model, string actionType)
        {
            // actionType может быть "encrypt" или "decrypt" по имени кнопки в форме
            if (string.IsNullOrWhiteSpace(model.InputText) || string.IsNullOrWhiteSpace(model.Algorithm))
            {
                ModelState.AddModelError("", "Пожалуйста, укажите текст и алгоритм.");
                return View(model);
            }

            try
            {
                if (model.Algorithm == "AES")
                {
                    if (actionType == "encrypt")
                    {
                        // Шифруем методом AES
                        // Если GenerateKey = true, генерируем новый ключ (key + iv), иначе читаем из model.Key (Base64: key:iv)
                        byte[] keyBytes, ivBytes;
                        if (model.GenerateKey || string.IsNullOrWhiteSpace(model.Key))
                        {
                            using (Aes aes = Aes.Create())
                            {
                                aes.GenerateKey();
                                aes.GenerateIV();
                                keyBytes = aes.Key;
                                ivBytes = aes.IV;
                            }
                            // Сохраним ключ+IV в виде Base64 для отображения пользователю: key:iv
                            string combined = Convert.ToBase64String(keyBytes) + ":" + Convert.ToBase64String(ivBytes);
                            model.Key = combined;
                        }
                        else
                        {
                            // Разбираем model.Key как "Base64Key:Base64IV"
                            var parts = model.Key.Split(':', 2);
                            keyBytes = Convert.FromBase64String(parts[0]);
                            ivBytes = Convert.FromBase64String(parts[1]);
                        }

                        byte[] encryptedBytes = EncryptAes(Encoding.UTF8.GetBytes(model.InputText), keyBytes, ivBytes);
                        model.ResultText = Convert.ToBase64String(encryptedBytes);
                    }
                    else if (actionType == "decrypt")
                    {
                        // Дешифруем AES: model.InputText = текст в Base64, model.Key = "Base64Key:Base64IV"
                        var parts = model.Key.Split(':', 2);
                        byte[] keyBytes = Convert.FromBase64String(parts[0]);
                        byte[] ivBytes = Convert.FromBase64String(parts[1]);
                        byte[] cipherBytes = Convert.FromBase64String(model.InputText);
                        byte[] plainBytes = DecryptAes(cipherBytes, keyBytes, ivBytes);
                        model.ResultText = Encoding.UTF8.GetString(plainBytes);
                    }
                }
                else if (model.Algorithm == "RSA")
                {
                    if (actionType == "encrypt")
                    {
                        RSAParameters rsaParams;
                        if (model.GenerateKey || string.IsNullOrWhiteSpace(model.Key))
                        {
                            using (RSA rsa = RSA.Create(2048))
                            {
                                // Сохраним пары ключей в формате XML
                                rsaParams = rsa.ExportParameters(true);
                                model.Key = rsa.ToXmlString(true); // содержит как закрытый, так и открытый ключ
                            }
                        }
                        else
                        {
                            // Извлекаем параметры из XML
                            using (RSA rsa = RSA.Create())
                            {
                                rsa.FromXmlString(model.Key);
                                rsaParams = rsa.ExportParameters(true);
                            }
                        }

                        // Теперь шифруем с помощью открытой части ключа
                        byte[] dataToEncrypt = Encoding.UTF8.GetBytes(model.InputText);
                        using (RSA rsa = RSA.Create())
                        {
                            rsa.ImportParameters(rsaParams);
                            byte[] encryptedBytes = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);
                            model.ResultText = Convert.ToBase64String(encryptedBytes);
                        }
                    }
                    else if (actionType == "decrypt")
                    {
                        // Дешифрование RSA: model.InputText – Base64, model.Key – XML со всеми параметрами (включая d)
                        using (RSA rsa = RSA.Create())
                        {
                            rsa.FromXmlString(model.Key);
                            byte[] cipherBytes = Convert.FromBase64String(model.InputText);
                            byte[] plainBytes = rsa.Decrypt(cipherBytes, RSAEncryptionPadding.Pkcs1);
                            model.ResultText = Encoding.UTF8.GetString(plainBytes);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Ошибка при шифровании/дешифровании: {ex.Message}");
            }

            return View(model);
        }

        // Вспомогательные методы для AES
        private static byte[] EncryptAes(byte[] plainBytes, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (var ms = new System.IO.MemoryStream())
                using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainBytes, 0, plainBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }

        private static byte[] DecryptAes(byte[] cipherBytes, byte[] key, byte[] iv)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.IV = iv;
                using (var ms = new System.IO.MemoryStream())
                using (var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    return ms.ToArray();
                }
            }
        }
    }
}
