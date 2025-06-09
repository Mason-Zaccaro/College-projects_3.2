using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.КТ
{
    internal class _1_2
    {
        public static void Main_()
        {
            Console.WriteLine("=== Параллельный подсчет символов в строках ===\n");

            // Создаем список строк для обработки (имитируем документ)
            List<string> documentLines = GenerateDocumentLines();

            Console.WriteLine($"Создан документ из {documentLines.Count} строк");
            Console.WriteLine("Первые 5 строк для примера:");

            for (int i = 0; i < Math.Min(5, documentLines.Count); i++)
            {
                string preview = documentLines[i].Length > 50
                    ? documentLines[i].Substring(0, 50) + "..."
                    : documentLines[i];
                Console.WriteLine($"  {i + 1}: {preview}");
            }

            Console.WriteLine("\n--- Параллельная обработка ---");

            // Засекаем время
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Параллельная обработка
            var parallelResults = ProcessStringsParallel(documentLines);

            stopwatch.Stop();
            long parallelTime = stopwatch.ElapsedMilliseconds;

            // Показываем результаты
            ShowResults(parallelResults, "Параллельная обработка");
            Console.WriteLine($"Время выполнения: {parallelTime} мс\n");

            // Для сравнения - последовательная обработка
            Console.WriteLine("--- Последовательная обработка ---");

            stopwatch.Restart();
            var sequentialResults = ProcessStringsSequential(documentLines);
            stopwatch.Stop();
            long sequentialTime = stopwatch.ElapsedMilliseconds;

            ShowResults(sequentialResults, "Последовательная обработка");
            Console.WriteLine($"Время выполнения: {sequentialTime} мс");

            // Сравнение производительности
            Console.WriteLine($"\n--- Сравнение ---");
            if (parallelTime > 0)
            {
                double speedup = (double)sequentialTime / parallelTime;
                Console.WriteLine($"Ускорение: {speedup:F2}x");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        // Генерируем список строк (имитируем документ)
        static List<string> GenerateDocumentLines()
        {
            Random random = new Random();
            List<string> lines = new List<string>();

            // Создаем разные типы строк
            string[] sampleTexts = {
                "Это обычная строка текста в документе.",
                "Здесь может быть описание какого-то процесса или алгоритма работы системы.",
                "Короткая строка",
                "Очень длинная строка с большим количеством символов, которая может содержать подробное описание, технические характеристики, инструкции и прочую полезную информацию для пользователя.",
                "123456789 - строка с цифрами и символами!@#$%^&*()",
                "Многоязычная строка: Hello, Привет, Bonjour, 你好",
                "",
                "   Строка с пробелами в начале и конце   "
            };

            // Генерируем много строк для наглядности
            for (int i = 0; i < 10000; i++)
            {
                // Берем случайный шаблон и немного его модифицируем
                string baseText = sampleTexts[random.Next(sampleTexts.Length)];
                string modifiedText = $"[Строка {i + 1}] {baseText}";

                // Иногда добавляем дополнительный текст
                if (random.Next(3) == 0)
                {
                    modifiedText += $" Дополнительная информация {random.Next(1000)}.";
                }

                lines.Add(modifiedText);
            }

            return lines;
        }

        // Параллельная обработка строк
        static List<StringResult> ProcessStringsParallel(List<string> strings)
        {
            Console.WriteLine($"Запуск параллельной обработки на {Environment.ProcessorCount} потоках...");

            // Список для результатов (потокобезопасный)
            var results = new List<StringResult>(strings.Count);

            // Инициализируем список нужного размера
            for (int i = 0; i < strings.Count; i++)
            {
                results.Add(new StringResult());
            }

            // Параллельная обработка каждой строки
            Parallel.ForEach(strings.Select((text, index) => new { Text = text, Index = index }), item =>
            {
                // Подсчитываем различные характеристики строки
                var result = AnalyzeString(item.Text, item.Index);
                results[item.Index] = result;

                // Показываем прогресс для первых нескольких строк
                if (item.Index < 10)
                {
                    Console.WriteLine($"Поток {Task.CurrentId}: обработана строка {item.Index + 1}");
                }
            });

            return results;
        }

        // Последовательная обработка строк (для сравнения)
        static List<StringResult> ProcessStringsSequential(List<string> strings)
        {
            var results = new List<StringResult>();

            for (int i = 0; i < strings.Count; i++)
            {
                var result = AnalyzeString(strings[i], i);
                results.Add(result);
            }

            return results;
        }

        // Анализ одной строки (имитируем более сложную обработку)
        static StringResult AnalyzeString(string text, int index)
        {
            // Имитируем более сложную обработку с небольшой задержкой
            System.Threading.Thread.Sleep(1); // 1 мс задержка

            var result = new StringResult
            {
                Index = index,
                OriginalText = text,
                TotalCharacters = text.Length,
                Letters = text.Count(char.IsLetter),
                Digits = text.Count(char.IsDigit),
                Spaces = text.Count(char.IsWhiteSpace),
                SpecialCharacters = text.Count(c => !char.IsLetterOrDigit(c) && !char.IsWhiteSpace(c)),
                Words = text.Split(new char[] { ' ', '\t', '\n' }, StringSplitOptions.RemoveEmptyEntries).Length
            };

            return result;
        }

        // Показываем результаты
        static void ShowResults(List<StringResult> results, string method)
        {
            if (results.Count == 0) return;

            int totalCharacters = results.Sum(r => r.TotalCharacters);
            int totalLetters = results.Sum(r => r.Letters);
            int totalDigits = results.Sum(r => r.Digits);
            int totalWords = results.Sum(r => r.Words);

            Console.WriteLine($"Результаты ({method}):");
            Console.WriteLine($"  Всего строк: {results.Count:N0}");
            Console.WriteLine($"  Всего символов: {totalCharacters:N0}");
            Console.WriteLine($"  Букв: {totalLetters:N0}");
            Console.WriteLine($"  Цифр: {totalDigits:N0}");
            Console.WriteLine($"  Слов: {totalWords:N0}");

            // Показываем первые несколько результатов для примера
            Console.WriteLine("\nПример результатов (первые 3 строки):");
            for (int i = 0; i < Math.Min(3, results.Count); i++)
            {
                var r = results[i];
                string preview = r.OriginalText.Length > 30
                    ? r.OriginalText.Substring(0, 30) + "..."
                    : r.OriginalText;
                Console.WriteLine($"  Строка {r.Index + 1}: '{preview}' -> {r.TotalCharacters} символов, {r.Words} слов");
            }
        }
    }
    public class StringResult
    {
        public int Index { get; set; }
        public string OriginalText { get; set; }
        public int TotalCharacters { get; set; }
        public int Letters { get; set; }
        public int Digits { get; set; }
        public int Spaces { get; set; }
        public int SpecialCharacters { get; set; }
        public int Words { get; set; }
    }
}
