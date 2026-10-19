using System;
using System.Text;

class Program
{
    // Параметри варіанта
    static int N = 28;
    static int K = 1;
    static int a = 1; // Для фільтрації: кратні (K % 4 + 2) -> кратні 3
    static int b = 0; // Масив: 20 випадкових чисел у діапазоні [-9; 9], матриця 4x5
    static int c = 3; // Перестановка: перенести від'ємні та нульові на початок, решту в кінець

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Оберіть завдання для виконання:");
            Console.WriteLine("1. Завдання 1: Агрегація масиву за один прохід");
            Console.WriteLine("2. Завдання 2: Фільтрація (новий масив точного розміру)");
            Console.WriteLine("3. Завдання 3: Пошук найдовшої серії однакових елементів");
            Console.WriteLine("4. Завдання 4: Перестановка на місці (від'ємні/нулі на початок)");
            Console.WriteLine("5. Завдання 5: Обробка матриці 4x5 за рядками та стовпцями");
            Console.WriteLine("6. Завдання 6: Перевірка крайових випадків (Завдання 1-4)");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string? choiceStr = Console.ReadLine();
            if (!int.TryParse(choiceStr, out int choice))
            {
                Console.WriteLine("Помилка! Введіть ціле число.\n");
                continue;
            }

            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    {
                        int[] arr = GenerateArray(N, 20, -9, 9);
                        Console.WriteLine("Згенерований масив: " + PrintArray(arr));
                        Task1Aggregation(arr);
                        break;
                    }
                case 2:
                    {
                        int[] arr = GenerateArray(N, 20, -9, 9);
                        Console.WriteLine("Згенерований масив: " + PrintArray(arr));
                        int divisor = (K % 4) + 2;
                        int[] filtered = Task2Filter(arr, divisor);
                        Console.WriteLine($"Відфільтровані елементи (кратні {divisor}): " + PrintArray(filtered));
                        break;
                    }
                case 3:
                    {
                        int[] arr = GenerateArray(N, 20, -9, 9);
                        Console.WriteLine("Згенерований масив: " + PrintArray(arr));
                        Task3LongestSeries(arr);
                        break;
                    }
                case 4:
                    {
                        int[] arr = GenerateArray(N, 20, -9, 9);
                        Console.WriteLine("Масив до перестановки:   " + PrintArray(arr));
                        Task4RearrangeInPlace(arr);
                        Console.WriteLine("Масив після перестановки: " + PrintArray(arr));
                        break;
                    }
                case 5:
                    {
                        Task5Matrix(N);
                        break;
                    }
                case 6:
                    {
                        Task6EdgeCases();
                        break;
                    }
                case 0:
                    return;
                default:
                    Console.WriteLine("Невірний вибір.\n");
                    break;
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    static int[] GenerateArray(int seed, int size, int minVal, int maxVal)
    {
        Random rnd = new Random(seed);
        int[] arr = new int[size];
        for (int i = 0; i < size; i++)
        {
            arr[i] = rnd.Next(minVal, maxVal + 1);
        }
        return arr;
    }

    static string PrintArray(int[] arr)
    {
        if (arr == null || arr.Length == 0) return "[поточний масив порожній]";
        return "[" + string.Join(", ", arr) + "]";
    }

    // ==========================================
    // ЗАВДАННЯ 1: Агрегація за один прохід
    // ==========================================
    static void Task1Aggregation(int[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Console.WriteLine("Помилка: масив порожній, агрегація не визначена.");
            return;
        }

        long sum = arr[0];
        int min = arr[0], max = arr[0];
        int minIndex = 0, maxIndex = 0;
        int zeroCount = arr[0] == 0 ? 1 : 0;

        for (int i = 1; i < arr.Length; i++)
        {
            int val = arr[i];
            sum += val;
            if (val == 0) zeroCount++;

            if (val < min)
            {
                min = val;
                minIndex = i;
            }
            if (val > max)
            {
                max = val;
                maxIndex = i;
            }
        }

        double average = (double)sum / arr.Length;

        Console.WriteLine($"\nРезультати агрегації:");
        Console.WriteLine($"- Сума: {sum}");
        Console.WriteLine($"- Середнє арифметичне: {average:F2}");
        Console.WriteLine($"- Мінімум: {min} (індекс {minIndex})");
        Console.WriteLine($"- Максимум: {max} (індекс {maxIndex})");
        Console.WriteLine($"- Кількість нулів: {zeroCount}");
    }

    // ==========================================
    // ЗАВДАННЯ 2: Фільтрація в новий масив
    // ==========================================
    static int[] Task2Filter(int[] arr, int divisor)
    {
        if (arr == null || arr.Length == 0) return new int[0];

        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] % divisor == 0) count++;
        }

        int[] result = new int[count];
        int index = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] % divisor == 0)
            {
                result[index++] = arr[i];
            }
        }

        return result;
    }

    // ==========================================
    // ЗАВДАННЯ 3: Найдовша серія однакових елементів
    // ==========================================
    static void Task3LongestSeries(int[] arr)
    {
        if (arr == null || arr.Length == 0)
        {
            Console.WriteLine("Масив порожній, серії відсутні.");
            return;
        }

        int bestVal = arr[0];
        int bestLen = 1;
        int bestStartIndex = 0;

        int currentVal = arr[0];
        int currentLen = 1;
        int currentStartIndex = 0;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] == currentVal)
            {
                currentLen++;
            }
            else
            {
                if (currentLen > bestLen)
                {
                    bestLen = currentLen;
                    bestVal = currentVal;
                    bestStartIndex = currentStartIndex;
                }
                currentVal = arr[i];
                currentLen = 1;
                currentStartIndex = i;
            }
        }

        if (currentLen > bestLen)
        {
            bestLen = currentLen;
            bestVal = currentVal;
            bestStartIndex = currentStartIndex;
        }

        Console.WriteLine($"\nНайдовша серія:");
        Console.WriteLine($"- Значення елемента: {bestVal}");
        Console.WriteLine($"- Довжина серії: {bestLen}");
        Console.WriteLine($"- Індекс початку: {bestStartIndex}");
    }

    // ==========================================
    // ЗАВДАННЯ 4: Перестановка на місці (c = 3)
    // ==========================================
    static void Task4RearrangeInPlace(int[] arr)
    {
        if (arr == null || arr.Length <= 1) return;

        int insertPos = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] <= 0)
            {
                int temp = arr[i];
                for (int j = i; j > insertPos; j--)
                {
                    arr[j] = arr[j - 1];
                }
                arr[insertPos] = temp;
                insertPos++;
            }
        }
    }

    // ==========================================
    // ЗАВДАННЯ 5: Обробка матриці (4 × 5)
    // ==========================================
    static void Task5Matrix(int seed)
    {
        Console.WriteLine("--- ЗАВДАННЯ 5: Обробка матриці 4 x 5 ---");
        int rows = 4;
        int cols = 5;
        int[,] matrix = new int[rows, cols];
        Random rnd = new Random(seed);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                matrix[i, j] = rnd.Next(-9, 10);
            }
        }

        Console.WriteLine("Матриця:");
        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                Console.Write($"{matrix[i, j],4} ");
            }
            Console.WriteLine();
        }

        Console.WriteLine("\nСуми рядків:");
        int maxRowSum = int.MinValue;
        int maxRowIndex = -1;

        for (int i = 0; i < matrix.GetLength(0); i++)
        {
            int rowSum = 0;
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                rowSum += matrix[i, j];
            }
            Console.WriteLine($"Рядок {i}: сума = {rowSum}");

            if (rowSum > maxRowSum)
            {
                maxRowSum = rowSum;
                maxRowIndex = i;
            }
        }
        Console.WriteLine($"\nРядок з найбільшою сумою: {maxRowIndex} (сума = {maxRowSum})");

        Console.WriteLine("\nМаксимуми стовпців:");
        for (int j = 0; j < matrix.GetLength(1); j++)
        {
            int colMax = matrix[0, j];
            for (int i = 1; i < matrix.GetLength(0); i++)
            {
                if (matrix[i, j] > colMax)
                {
                    colMax = matrix[i, j];
                }
            }
            Console.WriteLine($"Стовпець {j}: максимум = {colMax}");
        }
    }

    // ==========================================
    // ЗАВДАННЯ 6: Перевірка крайових випадків
    // ==========================================
    static void Task6EdgeCases()
    {
        Console.WriteLine("--- ЗАВДАННЯ 6: Тестування крайових випадків ---\n");

        int[][] testCases = new int[][]
        {
            new int[0],
            new int[] { 7 },
            new int[] { 5, 5, 5, 5 }
        };

        foreach (var tc in testCases)
        {
            string typeName = tc.Length == 0 ? "Порожній масив" : (tc.Length == 1 ? "Одноелементний" : "Усі однакові");
            Console.WriteLine($"\nТестуємо: {typeName} -> {PrintArray(tc)}");

            if (tc.Length == 0)
                Console.WriteLine("Завдання 1: [Коректно оброблено] Масив порожній, агрегація не визначена.");
            else
                Task1Aggregation(tc);

            int[] filtered = Task2Filter(tc, 3);
            Console.WriteLine("Завдання 2 (фільтр): " + PrintArray(filtered));

            if (tc.Length == 0)
                Console.WriteLine("Завдання 3: [Коректно оброблено] Масив порожній.");
            else
                Task3LongestSeries(tc);

            int[] clone = (int[])tc.Clone();
            Task4RearrangeInPlace(clone);
            Console.WriteLine("Завдання 4 (перестановка): " + PrintArray(clone));
            Console.WriteLine(new string('-', 40));
        }
    }
}