using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Base.КТ
{
    public class _4
    {
        public static void Main_()
        {
            Run().Wait();
        }

        static async Task Run()
        {
            int[,] A = GenerateMatrix(3, 2); // 3x2
            int[,] B = GenerateMatrix(2, 4); // 2x4

            Console.WriteLine("Матрица A:");
            PrintMatrix(A);
            Console.WriteLine("Матрица B:");
            PrintMatrix(B);

            var result = await MultiplyMatricesAsync(A, B);

            Console.WriteLine("Результат умножения A x B:");
            PrintMatrix(result);
        }

        static int[,] GenerateMatrix(int rows, int cols)
        {
            var rand = new Random();
            int[,] matrix = new int[rows, cols];
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                    matrix[i, j] = rand.Next(1, 10);
            return matrix;
        }

        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write(matrix[i, j] + "\t");
                Console.WriteLine();
            }
        }

        static async Task<int[,]> MultiplyMatricesAsync(int[,] A, int[,] B)
        {
            int m = A.GetLength(0);
            int n = A.GetLength(1);
            int p = B.GetLength(1);

            var result = new int[m, p];
            var queue = new ConcurrentQueue<(int row, int col)>();

            // 1. Первая стадия: подготовка координат
            for (int i = 0; i < m; i++)
                for (int j = 0; j < p; j++)
                    queue.Enqueue((i, j));

            // 2. Вторая стадия: конвейерное вычисление
            var tasks = new Task[4]; // 4 потока-рабочих
            for (int t = 0; t < tasks.Length; t++)
            {
                tasks[t] = Task.Run(() =>
                {
                    while (queue.TryDequeue(out var pos))
                    {
                        int sum = 0;
                        for (int k = 0; k < n; k++)
                        {
                            sum += A[pos.row, k] * B[k, pos.col];
                        }
                        result[pos.row, pos.col] = sum;
                    }
                });
            }

            await Task.WhenAll(tasks);

            return result;
        }
    }
}
