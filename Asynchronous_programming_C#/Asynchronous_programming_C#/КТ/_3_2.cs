using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Base.КТ
{
    public class _3_2
    {
        public static void Main_()
        {
            Run().Wait(); // Запускаем асинхронный метод синхронно
        }

        private static async Task Run()
        {
            int maxResources = 3;     // Кол-во ресурсов
            int numberOfTasks = 10;   // Кол-во задач

            SemaphoreSlim semaphore = new SemaphoreSlim(maxResources);
            Task[] tasks = new Task[numberOfTasks];

            Stopwatch stopwatch = Stopwatch.StartNew(); // Для измерения времени

            for (int i = 0; i < numberOfTasks; i++)
            {
                int taskId = i + 1;
                tasks[i] = Task.Run(async () =>
                {
                    Console.WriteLine($"[{stopwatch.ElapsedMilliseconds} мс] Задача {taskId} ждёт доступ...");

                    await semaphore.WaitAsync(); // Асинхронное ожидание

                    Console.WriteLine($"[{stopwatch.ElapsedMilliseconds} мс] Задача {taskId} получила доступ.");

                    await Task.Delay(2000); // Имитация работы

                    Console.WriteLine($"[{stopwatch.ElapsedMilliseconds} мс] Задача {taskId} завершила работу.");

                    semaphore.Release(); // Освобождаем ресурс
                });
            }

            await Task.WhenAll(tasks);

            stopwatch.Stop();

            Console.WriteLine($"Все задачи завершены за {stopwatch.Elapsed.TotalSeconds:F2} секунд.");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
