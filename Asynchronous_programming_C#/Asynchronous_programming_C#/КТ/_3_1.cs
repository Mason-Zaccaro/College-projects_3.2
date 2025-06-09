using System;
using System.Threading;
using System.Threading.Tasks;

namespace Base.КТ
{
    public class _3_1
    {
        public static void Main_()
        {
            Run().Wait(); // Ожидаем завершения асинхронного метода
        }

        private static async Task Run()
        {
            int maxResources = 3; // Кол-во ресурсов
            int numberOfTasks = 10; // Кол-во задач

            SemaphoreSlim semaphore = new SemaphoreSlim(maxResources);
            Task[] tasks = new Task[numberOfTasks];

            for (int i = 0; i < numberOfTasks; i++)
            {
                int taskId = i + 1;
                tasks[i] = Task.Run(async () =>
                {
                    Console.WriteLine($"Задача {taskId} пытается получить доступ к ресурсу...");
                    await semaphore.WaitAsync();

                    Console.WriteLine($"Задача {taskId} получила доступ к ресурсу!");
                    await Task.Delay(2000); // эмуляция работы
                    Console.WriteLine($"Задача {taskId} освобождает ресурс.");

                    semaphore.Release();
                });
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Все задачи завершены.");
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}
