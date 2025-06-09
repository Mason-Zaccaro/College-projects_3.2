using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Base.КТ
{
    public class _2_PipeClient
    {
        private const string PipeName = "MyMessagePipe";
        private const string ServerName = "."; // Локальный сервер
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

        public static async Task Main()
        {
            Console.WriteLine("=== ПРОЦЕСС-ОТПРАВИТЕЛЬ (CLIENT) ===");
            Console.WriteLine($"Подключение к каналу: {PipeName}");
            Console.WriteLine("Команды:");
            Console.WriteLine("  - Введите сообщение и нажмите Enter для отправки");
            Console.WriteLine("  - 'exit' - завершить работу");
            Console.WriteLine("  - 'help' - показать справку\n");

            NamedPipeClientStream pipeClient = null;

            try
            {
                // Создаем клиент именованного канала
                pipeClient = new NamedPipeClientStream(
                    ServerName,
                    PipeName,
                    PipeDirection.Out,    // Только для записи
                    PipeOptions.Asynchronous);

                Console.WriteLine("Подключение к серверу...");

                // Пытаемся подключиться с таймаутом
                await ConnectWithTimeout(pipeClient, 10000); // 10 секунд таймаут

                Console.WriteLine("Успешно подключились к серверу!");
                Console.WriteLine("Можете начинать отправку сообщений:\n");

                // Основной цикл отправки сообщений
                await SendMessagesLoop(pipeClient);
            }
            catch (TimeoutException)
            {
                Console.WriteLine("Не удалось подключиться к серверу (таймаут).");
                Console.WriteLine("Убедитесь, что сервер запущен и попробуйте снова.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Недостаточно прав доступа к именованному каналу.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка: {ex.Message}");
            }
            finally
            {
                // Корректно закрываем ресурсы
                try
                {
                    if (pipeClient != null)
                    {
                        if (pipeClient.IsConnected)
                        {
                            // Отправляем команду завершения
                            await SendMessage(pipeClient, "EXIT");
                        }
                        pipeClient.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при закрытии соединения: {ex.Message}");
                }

                Console.WriteLine("\nКлиент завершает работу...");
                Console.WriteLine("Нажмите любую клавишу для выхода.");
                Console.ReadKey();
            }
        }

        // Подключение с таймаутом
        private static async Task ConnectWithTimeout(NamedPipeClientStream pipeClient, int timeoutMs)
        {
            using (var timeoutCts = new CancellationTokenSource(timeoutMs))
            {
                try
                {
                    await pipeClient.ConnectAsync(timeoutCts.Token);
                }
                catch (OperationCanceledException) when (timeoutCts.Token.IsCancellationRequested)
                {
                    throw new TimeoutException($"Не удалось подключиться в течение {timeoutMs} мс");
                }
            }
        }

        // Основной цикл отправки сообщений
        private static async Task SendMessagesLoop(NamedPipeClientStream pipeClient)
        {
            try
            {
                while (pipeClient.IsConnected && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    Console.Write("Введите сообщение: ");

                    // Асинхронно читаем ввод пользователя
                    string input = await ReadLineAsync();

                    if (string.IsNullOrEmpty(input))
                        continue;

                    // Обрабатываем специальные команды
                    string command = input.Trim().ToLower();

                    if (command == "exit")
                    {
                        Console.WriteLine("Завершение работы...");
                        break;
                    }
                    else if (command == "help")
                    {
                        ShowHelp();
                        continue;
                    }
                    else if (command == "status")
                    {
                        ShowStatus(pipeClient);
                        continue;
                    }

                    // Отправляем сообщение
                    bool success = await SendMessage(pipeClient, input);

                    if (success)
                    {
                        Console.WriteLine("Сообщение отправлено");
                    }
                    else
                    {
                        Console.WriteLine("Ошибка отправки сообщения");
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Отправка сообщений отменена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка в цикле отправки: {ex.Message}");
            }
        }

        // Отправка сообщения
        private static async Task<bool> SendMessage(NamedPipeClientStream pipeClient, string message)
        {
            try
            {
                if (!pipeClient.IsConnected)
                {
                    Console.WriteLine("Соединение разорвано.");
                    return false;
                }

                // Конвертируем сообщение в байты
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);

                // Асинхронно отправляем данные
                await pipeClient.WriteAsync(messageBytes, 0, messageBytes.Length, cancellationTokenSource.Token);

                // Принудительно отправляем данные
                await pipeClient.FlushAsync(cancellationTokenSource.Token);

                return true;
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Отправка отменена.");
                return false;
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка отправки: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка отправки: {ex.Message}");
                return false;
            }
        }

        // Асинхронное чтение строки с консоли
        private static async Task<string> ReadLineAsync()
        {
            return await Task.Run(() =>
            {
                try
                {
                    return Console.ReadLine();
                }
                catch (Exception)
                {
                    return null;
                }
            });
        }

        // Показать справку
        private static void ShowHelp()
        {
            Console.WriteLine("\n=== СПРАВКА ===");
            Console.WriteLine("Доступные команды:");
            Console.WriteLine("  help   - показать эту справку");
            Console.WriteLine("  status - показать статус соединения");
            Console.WriteLine("  exit   - завершить работу");
            Console.WriteLine("Любой другой текст будет отправлен как сообщение.\n");
        }

        // Показать статус соединения
        private static void ShowStatus(NamedPipeClientStream pipeClient)
        {
            Console.WriteLine("\n=== СТАТУС СОЕДИНЕНИЯ ===");
            Console.WriteLine($"Подключен: {(pipeClient.IsConnected ? "Да" : "Нет")}");
            Console.WriteLine($"Канал: {PipeName}");
            Console.WriteLine($"Сервер: {ServerName}");
            Console.WriteLine();
        }
    }
}