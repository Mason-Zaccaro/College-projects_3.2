using System.Diagnostics;

namespace Base.��
{
    internal static class _1
    {
        public static async Task Main_()
        {
            Console.WriteLine("=== ������������ ������� ����� ������� ===\n");

            // ������� ������� ������ ��� ������������
            int arraySize = 1_000_000; 
            int[] numbers = GenerateArray(arraySize);

            Console.WriteLine($"������ ������ �� {arraySize:N0} ���������");
            Console.WriteLine("��������� ������� �����...\n");

            // �������� ����� ����������
            Stopwatch stopwatch = Stopwatch.StartNew();

            // ��������� ����� �����������
            long sum = await CalculateSumParallel(numbers);

            stopwatch.Stop();

            Console.WriteLine($"���������: {sum:N0}");
            Console.WriteLine($"����� ����������: {stopwatch.ElapsedMilliseconds} ��");

            // ��� ��������� - ������� �������
            Console.WriteLine("\n--- ��� ���������: ������� ������� ---");
            stopwatch.Restart();
            long normalSum = CalculateSumNormal(numbers);
            stopwatch.Stop();

            Console.WriteLine($"���������: {normalSum:N0}");
            Console.WriteLine($"����� ����������: {stopwatch.ElapsedMilliseconds} ��");

            Console.WriteLine("\n������� ����� ������� ��� ������...");
            Console.ReadKey();
        }

        // ���������� ������ ��������� �����
        static int[] GenerateArray(int size)
        {
            Random random = new Random();
            int[] array = new int[size];

            for (int i = 0; i < size; i++)
            {
                array[i] = random.Next(1, 100); // ����� �� 1 �� 99
            }

            return array;
        }

        // ������������ ������� �����
        static async Task<long> CalculateSumParallel(int[] array)
        {
            // ���������� ���������� ������� (������ ����� ���������� ���� ����������)
            int threadCount = Environment.ProcessorCount;
            Console.WriteLine($"������������ �������: {threadCount}");

            // ��������� ������ ������ ����� �������
            int chunkSize = array.Length / threadCount;

            // ������� ������ ��� ������� ������
            Task<long>[] tasks = new Task<long>[threadCount];

            for (int i = 0; i < threadCount; i++)
            {
                // ��������� ������� ��� ������� ����� �������
                int startIndex = i * chunkSize;
                int endIndex;

                // ��� ���������� ������ ����� ��� ���������� ��������
                if (i == threadCount - 1)
                {
                    endIndex = array.Length;
                }
                else
                {
                    endIndex = startIndex + chunkSize;
                }

                // ������� ������ ��� �������� ����� ����� �������
                int threadNumber = i + 1; // ��� ������ ����������
                tasks[i] = CalculatePartialSum(array, startIndex, endIndex, threadNumber);
            }

            // ���� ���������� ���� �����
            long[] partialSums = await Task.WhenAll(tasks);

            // ��������� ���������� ���� ������
            long totalSum = 0;
            for (int i = 0; i < partialSums.Length; i++)
            {
                totalSum += partialSums[i];
                Console.WriteLine($"����� {i + 1}: ����� = {partialSums[i]:N0}");
            }

            return totalSum;
        }

        // ������� ����� ����� ������� (����������� � ��������� ������)
        static async Task<long> CalculatePartialSum(int[] array, int startIndex, int endIndex, int threadNumber)
        {
            return await Task.Run(() =>
            {
                long partialSum = 0;

                for (int i = startIndex; i < endIndex; i++)
                {
                    partialSum += array[i];
                }

                Console.WriteLine($"����� {threadNumber}: ���������� ��������� {endIndex - startIndex:N0}");
                return partialSum;
            });
        }

        // ������� ������� ����� (��� ��������� ������������������)
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