using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Base.КТ
{
    public class _5_2
    {
        public static void Main_()
        {
            Run().Wait();
        }

        private static async Task Run()
        {
            Console.WriteLine("=== Асинхронная обработка данных из файла ===");
            string filePath = "numbers.txt"; // Укажи путь к файлу

            try
            {
                await foreach (var sum in ProcessFileAsync(filePath))
                {
                    Console.WriteLine($"Сумма в строке: {sum}");
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"Файл не найден: {ex.FileName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка обработки файла: {ex.Message}");
            }

            Console.WriteLine("Обработка завершена. Нажмите любую клавишу...");
            Console.ReadKey();
        }

        private static async IAsyncEnumerable<int> ProcessFileAsync(string path)
        {
            using var reader = new StreamReader(path);
            string? line;
            int lineNumber = 0;

            while ((line = await reader.ReadLineAsync()) != null)
            {
                lineNumber++;
                int sum = 0;
                try
                {
                    var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    foreach (var part in parts)
                    {
                        if (int.TryParse(part, out int number))
                        {
                            sum += number;
                        }
                        else
                        {
                            throw new FormatException($"Невозможно распознать число '{part}' в строке {lineNumber}.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка в строке {lineNumber}: {ex.Message}");
                    continue; // Переходим к следующей строке
                }

                yield return sum;
            }
        }
    }
}
