using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Base.КТ
{
    public class _3_3
    {
        public static void Main_()
        {
            Run().Wait();
        }

        private enum Priority { High, Low }

        private static async Task Run()
        {
            var semaphore = new SemaphoreSlim(2); // 2 ресурса

            var highPriorityQueue = new ConcurrentQueue<Func<Task>>();
            var lowPriorityQueue = new ConcurrentQueue<Func<Task>>();

            var random = new Random();
            var tasks = new Task[10];

            // Симулируем 10 задач с разным приоритетом
            for (int i = 0; i < 10; i++)
            {
                int taskId = i + 1;
                Priority priority = random.Next(0, 2) == 0 ? Priority.High : Priority.Low;

                Func<Task> taskWork = async () =>
                {
                    Console.WriteLine($"[{priority}] Задача {taskId} ожидает доступ...");

                    await semaphore.WaitAsync();

                    Console.WriteLine($"[{priority}] Задача {taskId} получила доступ.");

                    await Task.Delay(2000); // Работа с ресурсом

                    Console.WriteLine($"[{priority}] Задача {taskId} завершила работу.");

                    semaphore.Release();
                };

                if (priority == Priority.High)
                    highPriorityQueue.Enqueue(taskWork);
                else
                    lowPriorityQueue.Enqueue(taskWork);
            }

            async Task Scheduler()
            {
                while (!highPriorityQueue.IsEmpty || !lowPriorityQueue.IsEmpty)
                {
                    while (semaphore.CurrentCount > 0)
                    {
                        if (!highPriorityQueue.IsEmpty)
                        {
                            if (highPriorityQueue.TryDequeue(out var highTask))
                                _ = Task.Run(highTask);
                        }
                        else if (lowPriorityQueue.TryDequeue(out var lowTask))
                        {
                            _ = Task.Run(lowTask);
                        }
                        else break;
                    }

                    await Task.Delay(100);
                }
            }

            await Scheduler();

            Console.WriteLine("Все задачи завершены.");
            Console.ReadKey();
        }
    }
}
