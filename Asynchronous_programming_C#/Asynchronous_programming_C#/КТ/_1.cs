using System.Diagnostics;

namespace Base.КТ
{
    internal static class _1
    {
        public static async Task Main_()
        {
            Console.WriteLine("=== Параллельный подсчет суммы массива ===\n");

            // Создаем большой массив для тестирования
            int arraySize = 1_000_000; 
            int[] numbers = GenerateArray(arraySize);

            Console.WriteLine($"Создан массив из {arraySize:N0} элементов");
            Console.WriteLine("Запускаем подсчет суммы...\n");

            // Засекаем время выполнения
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Вычисляем сумму параллельно
            long sum = await CalculateSumParallel(numbers);

            stopwatch.Stop();

            Console.WriteLine($"Результат: {sum:N0}");
            Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");

            // Для сравнения - обычный подсчет
            Console.WriteLine("\n--- Для сравнения: обычный подсчет ---");
            stopwatch.Restart();
            long normalSum = CalculateSumNormal(numbers);
            stopwatch.Stop();

            Console.WriteLine($"Результат: {normalSum:N0}");
            Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");

            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }

        // Генерируем массив случайных чисел
        static int[] GenerateArray(int size)
        {
            Random random = new Random();
            int[] array = new int[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(1, 100); // числа от 1 до 99
            }

            return array;
        }

        // Параллельный подсчет суммы
        static async Task<long> CalculateSumParallel(int[] array)
        {
            // Определяем количество потоков (обычно равно количеству ядер процессора)
            int threadCount = Environment.ProcessorCount;
            Console.WriteLine($"Используется потоков: {threadCount}");

            // Вычисляем размер каждой части массива
            int chunkSize = array.Length / threadCount;

            // Создаем задачи для каждого потока
            Task<long>[] tasks = new Task<long>[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                // Вычисляем границы для текущей части массива
                int startIndex = i * chunkSize;
                int endIndex;

                // Для последнего потока берем все оставшиеся элементы
                if (i == threadCount - 1)
                {
                    endIndex = array.Length;
                }
                else
                {
                    endIndex = startIndex + chunkSize;
                }

                // Создаем задачу для подсчета суммы части массива
                int threadNumber = i + 1; // для вывода информации
                tasks[i] = CalculatePartialSum(array, startIndex, endIndex, threadNumber);
            }

            // Ждем завершения всех задач
            long[] partialSums = await Task.WhenAll(tasks);

            // Суммируем результаты всех частей
            long totalSum = 0;
            for (int i = 0; i < partialSums.Length; i++)
            {
                totalSum += partialSums[i];
                Console.WriteLine($"Поток {i + 1}: сумма = {partialSums[i]:N0}");
            }

            return totalSum;
        }

        // Подсчет суммы части массива (выполняется в отдельном потоке)
        static async Task<long> CalculatePartialSum(int[] array, int startIndex, int endIndex, int threadNumber)
        {
            return await Task.Run(() =>
            {
                long partialSum = 0;

                for (int i = startIndex; i < endIndex; i++)
                {
                    partialSum += array[i];
                }

                Console.WriteLine($"Поток {threadNumber}: обработано элементов {endIndex - startIndex:N0}");
                return partialSum;
            });
        }

        // Обычный подсчет суммы (для сравнения производительности)
        static long CalculateSumNormal(int[] array)
        {
            long sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                sum += array[i];
            }
            return sum;
        }
    }
}