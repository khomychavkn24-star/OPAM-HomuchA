using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

class Program
{
    // Параметри варіанта
    static int N = 28;
    static int K = 1;
    static int a = 1; // Завд. 2: кількість елементів, більших за K % 10 (1 % 10 = 1)
    static int b = 0; // Завд. 1: сумарний розмір усіх файлів у байтах
    static int c = 3; // Завд. 3: усі шляхи в матриці M x M (M = K % 3 + 3 = 1 % 3 + 3 = 3)

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Оберіть завдання для виконання:");
            Console.WriteLine("1. Завдання 1: Рекурсивний обхід каталогів (розмір файлів)");
            Console.WriteLine("2. Завдання 2: Розбиття навпіл (підрахунок елементів > 1)");
            Console.WriteLine("3. Завдання 3: Перебір із поверненням (шляхи в матриці 3x3)");
            Console.WriteLine("4. Завдання 4: Трасування стека викликів");
            Console.WriteLine("5. Завдання 5: Ітеративна версія та порівняння");
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
                        Console.Write("Введіть шлях до каталогу: ");
                        string? path = Console.ReadLine();
                        long totalSize = GetDirectorySizeRecursive(path);
                        if (totalSize >= 0)
                            Console.WriteLine($"\nСумарний розмір усіх файлів: {totalSize} байт");
                        break;
                    }
                case 2:
                    {
                        int[] arr = GenerateArray(N, 16, -20, 20);
                        Console.WriteLine("Масив (16 чисел [-20; 20]): " + PrintArray(arr));
                        int targetVal = K % 10; // 1
                        int maxDepth = 0;
                        int count = CountGreaterRecursive(arr, 0, arr.Length - 1, targetVal, 0, ref maxDepth);
                        int theoreticalLog = (int)Math.Ceiling(Math.Log2(arr.Length));
                        Console.WriteLine($"Кількість елементів, більших за {targetVal}: {count}");
                        Console.WriteLine($"Максимальна глибина рекурсії: {maxDepth} (теоретичний log₂16 = {theoreticalLog})");
                        break;
                    }
                case 3:
                    {
                        int M = (K % 3) + 3; // 3
                        Console.WriteLine($"Пошук усіх шляхів у матриці {M}x{M} з верхнього лівого в нижній правий:");
                        List<string> paths = new List<string>();
                        FindMatrixPaths(0, 0, M, M, "", paths);
                        foreach (var p in paths)
                        {
                            Console.WriteLine(p);
                        }
                        Console.WriteLine($"\nЗагальна кількість шляхів: {paths.Count}");
                        break;
                    }
                case 4:
                    {
                        int[] smallArr = GenerateArray(N, 4, -20, 20);
                        Console.WriteLine("Масив для трасування (4 елементи): " + PrintArray(smallArr));
                        Console.WriteLine("Трасування виконання функції знаходження максимуму:");
                        int tracedMax = TraceMaxRecursive(smallArr, 0, smallArr.Length - 1, 0);
                        Console.WriteLine($"Знайдений максимум: {tracedMax}");
                        break;
                    }
                case 5:
                    {
                        int[] arr = GenerateArray(N, 16, -20, 20);
                        int targetVal = K % 10;
                        int maxDepth = 0;
                        int recResult = CountGreaterRecursive(arr, 0, arr.Length - 1, targetVal, 0, ref maxDepth);
                        int iterResult = CountGreaterIterative(arr, targetVal);
                        Console.WriteLine($"Результат рекурсії: {recResult}");
                        Console.WriteLine($"Результат циклу:     {iterResult}");
                        Console.WriteLine("Збіг результатів: " + (recResult == iterResult ? "ТАК" : "НІ"));
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
        if (arr == null || arr.Length == 0) return "[]";
        return "[" + string.Join(", ", arr) + "]";
    }

    // ==========================================
    // ЗАВДАННЯ 1: Обхід каталогів
    // ==========================================
    static long GetDirectorySizeRecursive(string? path)
    {
        if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
        {
            Console.WriteLine("Помилка: вказаного каталогу не існує або шлях порожній.");
            return -1;
        }

        long totalSize = 0;

        try
        {
            // Сумуємо розміри файлів у поточному каталозі
            string[] files = Directory.GetFiles(path);
            foreach (var file in files)
            {
                try
                {
                    FileInfo fi = new FileInfo(file);
                    totalSize += fi.Length;
                }
                catch (UnauthorizedAccessException)
                {
                    // Пропускаємо файл, до якого немає доступу
                }
            }

            // Рекурсивно обходимо підкаталоги
            string[] subdirs = Directory.GetDirectories(path);
            foreach (var subdir in subdirs)
            {
                long subSize = GetDirectorySizeRecursive(subdir);
                if (subSize > 0)
                {
                    totalSize += subSize;
                }
            }
        }
        catch (UnauthorizedAccessException)
        {
            // Обробка відсутності доступу до каталогу за правилами завдання
            // Мовчки пропускаємо або виводимо попередження
        }

        return totalSize;
    }

    // ==========================================
    // ЗАВДАННЯ 2: Розбиття навпіл (підрахунок > val)
    // ==========================================
    static int CountGreaterRecursive(int[] arr, int lo, int hi, int targetVal, int currentDepth, ref int maxDepth)
    {
        if (currentDepth > maxDepth) maxDepth = currentDepth;

        // Базовий випадок 1: порожній діапазон
        if (lo > hi) return 0;

        // Базовий випадок 2: один елемент
        if (lo == hi)
        {
            return arr[lo] > targetVal ? 1 : 0;
        }

        int mid = lo + (hi - lo) / 2;
        int leftCount = CountGreaterRecursive(arr, lo, mid, targetVal, currentDepth + 1, ref maxDepth);
        int rightCount = CountGreaterRecursive(arr, mid + 1, hi, targetVal, currentDepth + 1, ref maxDepth);

        return leftCount + rightCount;
    }

    // ==========================================
    // ЗАВДАННЯ 3: Перебір із поверненням (шляхи в матриці M x M)
    // ==========================================
    static void FindMatrixPaths(int r, int c, int rows, int cols, string currentPath, List<string> allPaths)
    {
        // Базовий випадок: дійшли до нижнього правого кута
        string step = $"({r},{c})";
        string pathSoFar = string.IsNullOrEmpty(currentPath) ? step : currentPath + " -> " + step;

        if (r == rows - 1 && c == cols - 1)
        {
            allPaths.Add(pathSoFar);
            return;
        }

        // Рух вправо
        if (c + 1 < cols)
        {
            FindMatrixPaths(r, c + 1, rows, cols, pathSoFar, allPaths);
        }

        // Рух вниз
        if (r + 1 < rows)
        {
            FindMatrixPaths(r + 1, c, rows, cols, pathSoFar, allPaths);
        }
    }

    // ==========================================
    // ЗАВДАННЯ 4: Трасування стека викликів (для Максимуму)
    // ==========================================
    static int TraceMaxRecursive(int[] arr, int lo, int hi, int depth)
    {
        string indent = new string(' ', depth * 2);
        Console.WriteLine($"{indent}enter Max({lo}..{hi})");

        if (lo == hi)
        {
            int val = arr[lo];
            Console.WriteLine($"{indent}  -> base case, value = {val}");
            Console.WriteLine($"{indent}exit  Max({lo}..{hi}) -> {val}");
            return val;
        }

        int mid = lo + (hi - lo) / 2;
        int leftMax = TraceMaxRecursive(arr, lo, mid, depth + 1);
        int rightMax = TraceMaxRecursive(arr, mid + 1, hi, depth + 1);

        int maxVal = Math.Max(leftMax, rightMax);
        Console.WriteLine($"{indent}exit  Max({lo}..{hi}) -> {maxVal}");
        return maxVal;
    }

    // ==========================================
    // ЗАВДАННЯ 5: Ітеративна версія
    // ==========================================
    static int CountGreaterIterative(int[] arr, int targetVal)
    {
        int count = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            if (arr[i] > targetVal)
            {
                count++;
            }
        }
        return count;
    }
}