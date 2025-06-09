using System;
using System.IO;
using System.IO.Pipes;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Base.КТ
{
    public class _2_PipeServer
    {
        private const string PipeName = "MyMessagePipe";
        private static CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private static bool isRunning = true;

        public static async Task Main()
        {
            Console.WriteLine("=== ПРОЦЕСС-ПОЛУЧАТЕЛЬ (SERVER) ===");
            Console.WriteLine($"Именованный канал: {PipeName}");
            Console.WriteLine("Ожидание подключения клиентов...");
            Console.WriteLine("Нажмите 'Q' для выхода\n");

            // Запускаем мониторинг клавиш в отдельном потоке
            _ = Task.Run(MonitorKeyPress);

            try
            {
                // Основной цикл работы сервера
                while (isRunning && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    await HandleClientConnection();

                    // Небольшая пауза перед следующим подключением
                    if (isRunning)
                    {
                        Console.WriteLine("Ожидание нового подключения...\n");
                        await Task.Delay(1000, cancellationTokenSource.Token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Операция отменена пользователем.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Критическая ошибка: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("\nСервер завершает работу...");
                Console.WriteLine("Нажмите любую клавишу для выхода.");
                Console.ReadKey();
            }
        }

        // Обработка подключения одного клиента
        private static async Task HandleClientConnection()
        {
            NamedPipeServerStream pipeServer = null;

            try
            {
                // Создаем сервер именованного канала
                pipeServer = new NamedPipeServerStream(
                    PipeName,
                    PipeDirection.In,     // Только для чтения
                    1,                    // Максимум 1 клиент одновременно
                    PipeTransmissionMode.Byte,
                    PipeOptions.Asynchronous);

                Console.WriteLine("Канал создан. Ожидание подключения клиента...");

                // Асинхронно ждем подключения клиента
                await pipeServer.WaitForConnectionAsync(cancellationTokenSource.Token);

                Console.WriteLine("Клиент подключился!");
                Console.WriteLine("Начинаем прием сообщений:\n");

                // Читаем сообщения от клиента
                await ReadMessagesFromClient(pipeServer);
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Подключение отменено.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка ввода-вывода: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке клиента: {ex.Message}");
            }
            finally
            {
                // Корректно закрываем ресурсы
                try
                {
                    if (pipeServer != null)
                    {
                        if (pipeServer.IsConnected)
                        {
                            pipeServer.Disconnect();
                            Console.WriteLine("Клиент отключен.");
                        }
                        pipeServer.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при закрытии канала: {ex.Message}");
                }
            }
        }

        // Чтение сообщений от клиента
        private static async Task ReadMessagesFromClient(NamedPipeServerStream pipeServer)
        {
            byte[] buffer = new byte[1024];

            try
            {
                while (pipeServer.IsConnected && isRunning && !cancellationTokenSource.Token.IsCancellationRequested)
                {
                    // Асинхронно читаем данные
                    int bytesRead = await pipeServer.ReadAsync(buffer, 0, buffer.Length, cancellationTokenSource.Token);

                    if (bytesRead > 0)
                    {
                        // Конвертируем байты в строку
                        string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        // Проверяем на команду завершения
                        if (message.Trim().ToUpper() == "EXIT")
                        {
                            Console.WriteLine("Получена команда завершения от клиента.");
                            break;
                        }

                        // Выводим полученное сообщение
                        string timestamp = DateTime.Now.ToString("HH:mm:ss");
                        Console.WriteLine($"[{timestamp}] Получено: {message}");
                    }
                    else
                    {
                        // Клиент закрыл соединение
                        Console.WriteLine("Клиент закрыл соединение.");
                        break;
                    }
                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Чтение сообщений отменено.");
            }
            catch (IOException ex) when (ex.Message.Contains("pipe has been ended"))
            {
                Console.WriteLine("Клиент разорвал соединение.");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Ошибка чтения: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Неожиданная ошибка при чтении: {ex.Message}");
            }
        }

        // Мониторинг нажатий клавиш
        private static async Task MonitorKeyPress()
        {
            await Task.Run(() =>
            {
                try
                {
                    while (isRunning)
                    {
                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(true);
                            if (key.Key == ConsoleKey.Q)
                            {
                                Console.WriteLine("\n Получен сигнал завершения работы...");
                                isRunning = false;
                                cancellationTokenSource.Cancel();
                                break;
                            }
                        }
                        Thread.Sleep(100); // Небольшая задержка
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка мониторинга клавиш: {ex.Message}");
                }
            });
        }
    }
}