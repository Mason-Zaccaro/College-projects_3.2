using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Base.КТ
{
    public class _5_1
   {
        public static void Main_()
    {
        Run().Wait();
    }

    private static async Task Run()
    {
        Console.WriteLine("========== Асинхронный стрим Фибоначчи ==========");
        try
        {
            await foreach (var number in GenerateFibonacciAsync(10))
            {
                Console.WriteLine($"Число Фибоначчи: {number}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при генерации: {ex.Message}");
        }

        Console.WriteLine("Генерация завершена. Нажмите любую клавишу для выхода...");
        Console.ReadKey();
    }

    private static async IAsyncEnumerable<int> GenerateFibonacciAsync(int count)
    {
        int previous = 0;
        int current = 1;
        int delay = 500; // начальная задержка в миллисекундах

        for (int i = 0; i < count; i++)
        {
            // Генерируем следующее число
            int next = (i < 2) ? i : previous + current;
            if (i >= 2)
            {
                previous = current;
                current = next;
            }

            // Задержка перед возвратом
            await Task.Delay(delay);
            delay += 500; // увеличиваем задержку

            yield return next;
        }
    }
}
}