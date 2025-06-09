using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.КТ
{
    internal class _1_3
    {
        public static void Main_()
        {
            Console.WriteLine("=== Параллельный поиск минимума и максимума ===\n");

            // Создаем большой массив
            int arraySize = 50_000_000; // 50 миллионов элементов
            Console.WriteLine($"Генерация массива из {arraySize:N0} элементов...");

            int[] numbers = GenerateArray(arraySize);
            Console.WriteLine("Массив создан!\n");

            // Количество тестовых запусков
            int testRuns = 5;

            Console.WriteLine($"Проводим {testRuns} тестовых запусков для точности измерений:\n");

            // Массивы для хранения времени выполнения
            long[] sequentialTimes = new long[testRuns];
            long[] parallelTimes = new long[testRuns];

            // Переменные для результатов (для проверки корректности)
            int sequentialMin = 0, sequentialMax = 0;
            int parallelMin = 0, parallelMax = 0;

            // Серия тестов
            for (int run = 1; run <= testRuns; run++)
            {
                Console.WriteLine($"--- Запуск {run}/{testRuns} ---");

                // Тест последовательного алгоритма
                Console.Write("Последовательный поиск... ");
                Stopwatch sw = Stopwatch.StartNew();

                (sequentialMin, sequentialMax) = FindMinMaxSequential(numbers);

                sw.Stop();
                sequentialTimes[run - 1] = sw.ElapsedMilliseconds;
                Console.WriteLine($"{sw.ElapsedMilliseconds} мс");

                // Небольшая пауза между тестами
                System.Threading.Thread.Sleep(100);

                // Тест параллельного алгоритма
                Console.Write("Параллельный поиск...     ");
                sw.Restart();

                (parallelMin, parallelMax) = FindMinMaxParallel(numbers);

                sw.Stop();
                parallelTimes[run - 1] = sw.ElapsedMilliseconds;
                Console.WriteLine($"{sw.ElapsedMilliseconds} мс");

                // Проверяем корректность результатов
                if (sequentialMin != parallelMin || sequentialMax != parallelMax)
                {
                    Console.WriteLine("ОШИБКА: результаты не совпадают!");
                    return;
                }

                Console.WriteLine($"Результат: Min = {sequentialMin:N0}, Max = {sequentialMax:N0}\n");
            }

            // Анализ результатов
            AnalyzeResults(sequentialTimes, parallelTimes);
        }

        // Генерация массива случайных чисел
        static int[] GenerateArray(int size)
        {
            Random random = new Random(42); // Фиксированный seed для воспроизводимости
            int[] array = new int[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(-1_000_000, 1_000_000); // от -1 млн до 1 млн
            }

            return array;
        }

        // Последовательный поиск минимума и максимума
        static (int min, int max) FindMinMaxSequential(int[] array)
        {
            if (array.Length == 0)
                throw new ArgumentException("Массив не может быть пустым");

            int min = array[0];
            int max = array[0];

            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < min)
                    min = array[i];
                else if (array[i] > max)
                    max = array[i];
            }

            return (min, max);
        }

        // Параллельный поиск минимума и максимума
        static (int min, int max) FindMinMaxParallel(int[] array)
        {
            if (array.Length == 0)
                throw new ArgumentException("Массив не может быть пустым");

            // Определяем количество потоков
            int threadCount = Environment.ProcessorCount;
            int chunkSize = array.Length / threadCount;

            // Создаем задачи для каждого потока
            Task<(int min, int max)>[] tasks = new Task<(int min, int max)>[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                int startIndex = i * chunkSize;
                int endIndex = (i == threadCount - 1) ? array.Length : startIndex + chunkSize;

                // Создаем задачу для поиска в части массива
                tasks[i] = Task.Run(() => FindMinMaxInRange(array, startIndex, endIndex));
            }

            // Ждем завершения всех задач
            Task.WaitAll(tasks);

            // Находим общий минимум и максимум из результатов всех потоков
            int globalMin = tasks[0].Result.min;
            int globalMax = tasks[0].Result.max;

            for (int i = 1; i < tasks.Length; i++)
            {
                var result = tasks[i].Result;
                if (result.min < globalMin)
                    globalMin = result.min;
                if (result.max > globalMax)
                    globalMax = result.max;
            }

            return (globalMin, globalMax);
        }

        // Поиск минимума и максимума в диапазоне индексов
        static (int min, int max) FindMinMaxInRange(int[] array, int startIndex, int endIndex)
        {
            int min = array[startIndex];
            int max = array[startIndex];

            for (int i = startIndex + 1; i < endIndex; i++)
            {
                if (array[i] < min)
                    min = array[i];
                else if (array[i] > max)
                    max = array[i];
            }

            return (min, max);
        }

        // Анализ результатов производительности
        static void AnalyzeResults(long[] sequentialTimes, long[] parallelTimes)
        {
            Console.WriteLine("=== АНАЛИЗ ПРОИЗВОДИТЕЛЬНОСТИ ===\n");

            // Вычисляем статистики
            double avgSequential = sequentialTimes.Average();
            double avgParallel = parallelTimes.Average();

            long minSequential = sequentialTimes.Min();
            long maxSequential = sequentialTimes.Max();
            long minParallel = parallelTimes.Min();
            long maxParallel = parallelTimes.Max();

            // Выводим подробную статистику
            Console.WriteLine("Результаты всех запусков:");
            Console.WriteLine("Запуск | Последовательно | Параллельно | Ускорение");
            Console.WriteLine("-------|------------------|-------------|----------");

            for (int i = 0; i < sequentialTimes.Length; i++)
            {
                double speedup = (double)sequentialTimes[i] / parallelTimes[i];
                Console.WriteLine($"   {i + 1}   |     {sequentialTimes[i],6} мс    |   {parallelTimes[i],6} мс  |   {speedup:F2}x");
            }

            Console.WriteLine("\nСтатистика:");
            Console.WriteLine($"Последовательный алгоритм:");
            Console.WriteLine($"  Среднее время: {avgSequential:F1} мс");
            Console.WriteLine($"  Минимальное:   {minSequential} мс");
            Console.WriteLine($"  Максимальное:  {maxSequential} мс");

            Console.WriteLine($"\nПараллельный алгоритм:");
            Console.WriteLine($"  Среднее время: {avgParallel:F1} мс");
            Console.WriteLine($"  Минимальное:   {minParallel} мс");
            Console.WriteLine($"  Максимальное:  {maxParallel} мс");

            // Вычисляем ускорение
            double avgSpeedup = avgSequential / avgParallel;
            Console.WriteLine($"\n--- ИТОГОВЫЙ РЕЗУЛЬТАТ ---");
            Console.WriteLine($"Среднее ускорение: {avgSpeedup:F2}x");
            Console.WriteLine($"Процессорных ядер: {Environment.ProcessorCount}");

            // Выводы и обоснование
            Console.WriteLine("\n=== ВЫВОД-ОБОСНОВАНИЕ ===");

            if (avgSpeedup > 1.5)
            {
                Console.WriteLine("ПАРАЛЛЕЛИЗМ ЭФФЕКТИВЕН:");
                Console.WriteLine($"  • Получено значительное ускорение в {avgSpeedup:F2} раза");
                Console.WriteLine($"  • Эффективность использования ядер: {(avgSpeedup / Environment.ProcessorCount * 100):F1}%");
            }
            else if (avgSpeedup > 1.1)
            {
                Console.WriteLine("ПАРАЛЛЕЛИЗМ УМЕРЕННО ЭФФЕКТИВЕН:");
                Console.WriteLine($"  • Получено небольшое ускорение в {avgSpeedup:F2} раза");
                Console.WriteLine("  • Накладные расходы на управление потоками частично съедают выигрыш");
            }
            else
            {
                Console.WriteLine("ПАРАЛЛЕЛИЗМ НЕЭФФЕКТИВЕН:");
                Console.WriteLine($"  • Ускорение менее {avgSpeedup:F2} раза");
                Console.WriteLine("  • Накладные расходы превышают выигрыш от параллелизма");
            }

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
