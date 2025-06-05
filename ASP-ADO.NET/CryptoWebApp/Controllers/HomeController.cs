using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using CryptoWebApp.Models;

namespace CryptoWebApp.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View(new CryptoModel());
        }

        [HttpPost]
        public IActionResult Encrypt(CryptoModel model)
        {
            try
            {
                if (model.Algorithm == "AES")
                {
                    if (string.IsNullOrEmpty(model.Key))
                    {
                        model.Key = GenerateAESKey();
                    }
                    model.EncryptedText = EncryptAES(model.Text, model.Key);
                    model.DecryptedText = DecryptAES(model.EncryptedText, model.Key);
                }
                else if (model.Algorithm == "RSA")
                {
                    var (publicKey, privateKey) = GenerateRSAKeys();
                    model.Key = $"Public: {publicKey}\nPrivate: {privateKey}";
                    model.EncryptedText = EncryptRSA(model.Text, publicKey);
                    model.DecryptedText = DecryptRSA(model.EncryptedText, privateKey);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
            }

            return View("Index", model);
        }

        private string GenerateAESKey()
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                return Convert.ToBase64String(aes.Key);
            }
        }

        private string EncryptAES(string text, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);
                aes.GenerateIV();

                using (var encryptor = aes.CreateEncryptor())
                using (var ms = new MemoryStream())
                {
                    ms.Write(aes.IV, 0, aes.IV.Length);
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(text);
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string DecryptAES(string encryptedText, string key)
        {
            var data = Convert.FromBase64String(encryptedText);
            using (var aes = Aes.Create())
            {
                aes.Key = Convert.FromBase64String(key);

                var iv = new byte[16];
                Array.Copy(data, 0, iv, 0, 16);
                aes.IV = iv;

                using (var decryptor = aes.CreateDecryptor())
                using (var ms = new MemoryStream(data, 16, data.Length - 16))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        private (string publicKey, string privateKey) GenerateRSAKeys()
        {
            using (var rsa = RSA.Create(2048))
            {
                var publicKey = Convert.ToBase64String(rsa.ExportRSAPublicKey());
                var privateKey = Convert.ToBase64String(rsa.ExportRSAPrivateKey());
                return (publicKey, privateKey);
            }
        }

        private string EncryptRSA(string text, string publicKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                var data = Encoding.UTF8.GetBytes(text);
                var encrypted = rsa.Encrypt(data, RSAEncryptionPadding.OaepSHA256);
                return Convert.ToBase64String(encrypted);
            }
        }

        private string DecryptRSA(string encryptedText, string privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                var data = Convert.FromBase64String(encryptedText);
                var decrypted = rsa.Decrypt(data, RSAEncryptionPadding.OaepSHA256);
                return Encoding.UTF8.GetString(decrypted);
            }
        }
    }
}