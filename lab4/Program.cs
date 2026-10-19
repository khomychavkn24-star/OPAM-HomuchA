using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

class Program
{
    // Параметри варіанта
    static int N = 28;
    static int K = 1;
    static int a = 1; // Завдання 1: елементи, кратні (K % 4 + 2) -> кратні 3
    static int b = 0; // Завдання 4: розділити на від'ємні та невід'ємні
    static int c = 3; // Завдання 2: вилучити елементи, що дорівнюють початковому першому елементу списку

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Оберіть завдання для виконання:");
            Console.WriteLine("1. Завдання 1: Побудова нового списку за критерієм");
            Console.WriteLine("2. Завдання 2: Вилучення елементів (3 варіанти)");
            Console.WriteLine("3. Завдання 3: Злиття двох упорядкованих списків");
            Console.WriteLine("4. Завдання 4: Розділення списку на два");
            Console.WriteLine("5. Завдання 5: Замір часу (Stopwatch) та таблиця");
            Console.WriteLine("6. Завдання 6: Перевірка крайових випадків");
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
                        List<int> mainList = GenerateMainList(N);
                        Console.WriteLine("Основний список: " + PrintList(mainList));
                        int divisor = (K % 4) + 2; // 3
                        List<int> filtered = Task1Filter(mainList, divisor);
                        Console.WriteLine($"Відібрані елементи (кратні {divisor}): " + PrintList(filtered) + $" (Count = {filtered.Count})");
                        Console.WriteLine("Перевірка незмінності основного: " + PrintList(mainList));
                        break;
                    }
                case 2:
                    {
                        List<int> mainList = GenerateMainList(N);
                        Console.WriteLine("Основний список: " + PrintList(mainList));
                        Task2RemoveVariants(mainList);
                        break;
                    }
                case 3:
                    {
                        Task3MergeLists();
                        break;
                    }
                case 4:
                    {
                        List<int> mainList = GenerateMainList(N);
                        Console.WriteLine("Основний список: " + PrintList(mainList));
                        Task4Split(mainList);
                        break;
                    }
                case 5:
                    {
                        Task5Benchmarks();
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

    // Генератор основного списку (20 чисел [-9; 9])
    static List<int> GenerateMainList(int seed)
    {
        Random rnd = new Random(seed);
        List<int> list = new List<int>(20);
        for (int i = 0; i < 20; i++)
        {
            list.Add(rnd.Next(-9, 10));
        }
        return list;
    }

    static string PrintList(List<int> list)
    {
        if (list == null || list.Count == 0) return "[]";
        return "[" + string.Join(", ", list) + "]";
    }

    // ==========================================
    // ЗАВДАННЯ 1: Побудова нового списку за критерієм
    // ==========================================
    static List<int> Task1Filter(List<int> src, int divisor)
    {
        if (src == null) return new List<int>();

        // Попередній підрахунок кількості для задання місткості (пункт 4 правил)
        int count = 0;
        for (int i = 0; i < src.Count; i++)
        {
            if (src[i] % divisor == 0) count++;
        }

        List<int> result = new List<int>(count);
        for (int i = 0; i < src.Count; i++)
        {
            if (src[i] % divisor == 0)
            {
                result.Add(src[i]);
            }
        }
        return result;
    }

    // ==========================================
    // ЗАВДАННЯ 2: Вилучення елементів (3 варіанти)
    // ==========================================
    static void Task2RemoveVariants(List<int> original)
    {
        if (original.Count == 0) return;
        int target = original[0];
        Console.WriteLine($"Цільове значення для вилучення (початковий перший елемент): {target}\n");

        // --- Варіант 1: Хибний (цикл від початку з RemoveAt) ---
        List<int> list1 = new List<int>(original);
        for (int i = 0; i < list1.Count; i++)
        {
            if (list1[i] == target)
            {
                list1.RemoveAt(i);
                // Помилка: індекс не зменшується, тому наступний елемент після видаленого пропускається!
            }
        }
        Console.WriteLine("1. Хибний варіант (з початку):    " + PrintList(list1));
        Console.WriteLine("   (Пропускає елементи, що йдуть одразу за видаленими через зсув індексів)");

        // --- Варіант 2: Правильний (цикл з кінця) ---
        List<int> list2 = new List<int>(original);
        for (int i = list2.Count - 1; i >= 0; i--)
        {
            if (list2[i] == target)
            {
                list2.RemoveAt(i);
            }
        }
        Console.WriteLine("2. Правильний (з кінця):        " + PrintList(list2));

        // --- Варіант 3: Правильний (новий список) ---
        List<int> list3 = new List<int>();
        for (int i = 0; i < original.Count; i++)
        {
            if (original[i] != target)
            {
                list3.Add(original[i]);
            }
        }
        Console.WriteLine("3. Правильний (новий список):   " + PrintList(list3));

        // Перевірка ідентичності результатів варіантів 2 і 3
        bool areEqual = (list2.Count == list3.Count);
        if (areEqual)
        {
            for (int i = 0; i < list2.Count; i++)
            {
                if (list2[i] != list3[i]) { areEqual = false; break; }
            }
        }
        Console.WriteLine($"\nПеревірка збігу результатів варіантів 2 і 3: {areEqual}");
    }

    // ==========================================
    // ЗАВДАННЯ 3: Злиття двох упорядкованих списків
    // ==========================================
    static void Task3MergeLists()
    {
        Console.WriteLine("--- ЗАВДАННЯ 3: Злиття впорядкованих списків ---");
        int len1 = 10 + (K % 5); // 10
        int len2 = 6 + (K % 4);  // 6

        // Генерація першого списку
        List<int> list1 = new List<int>(len1);
        Random rnd1 = new Random(N);
        int current = rnd1.Next(1, 10);
        list1.Add(current);
        for (int i = 1; i < len1; i++)
        {
            current += rnd1.Next(1, 10);
            list1.Add(current);
        }

        // Генерація другого списку
        List<int> list2 = new List<int>(len2);
        Random rnd2 = new Random(N + 100);
        current = rnd2.Next(1, 10);
        list2.Add(current);
        for (int i = 1; i < len2; i++)
        {
            current += rnd2.Next(1, 10);
            list2.Add(current);
        }

        Console.WriteLine("Список 1: " + PrintList(list1));
        Console.WriteLine("Список 2: " + PrintList(list2));

        // Злиття за один прохід двома індексами
        List<int> merged = new List<int>(len1 + len2);
        int idx1 = 0, idx2 = 0;

        while (idx1 < list1.Count && idx2 < list2.Count)
        {
            if (list1[idx1] <= list2[idx2])
            {
                merged.Add(list1[idx1]);
                idx1++;
            }
            else
            {
                merged.Add(list2[idx2]);
                idx2++;
            }
        }

        // Дописуємо хвіст першого або другого списку
        while (idx1 < list1.Count)
        {
            merged.Add(list1[idx1]);
            idx1++;
        }
        while (idx2 < list2.Count)
        {
            merged.Add(list2[idx2]);
            idx2++;
        }

        Console.WriteLine("Злий список: " + PrintList(merged));

        // Перевірка впорядкованості
        bool isSorted = true;
        for (int i = 0; i < merged.Count - 1; i++)
        {
            if (merged[i] > merged[i + 1])
            {
                isSorted = false;
                break;
            }
        }
        Console.WriteLine($"Перевірка впорядкованості (true/false): {isSorted}");
    }

    // ==========================================
    // ЗАВДАННЯ 4: Розділення списку на два
    // ==========================================
    static void Task4Split(List<int> src)
    {
        // b = 0: розділити на від'ємні та невід'ємні
        List<int> negatives = new List<int>();
        List<int> nonNegatives = new List<int>();

        for (int i = 0; i < src.Count; i++)
        {
            if (src[i] < 0) negatives.Add(src[i]);
            else nonNegatives.Add(src[i]);
        }

        Console.WriteLine("Від'ємні:    " + PrintList(negatives));
        Console.WriteLine("Невід'ємні:  " + PrintList(nonNegatives));

        bool sumCheck = (negatives.Count + nonNegatives.Count == src.Count);
        Console.WriteLine($"Перевірка суми Count (знизу дорівнює вихідному): {sumCheck}");
    }

    // ==========================================
    // ЗАВДАННЯ 5: Замір часу (Stopwatch)
    // ==========================================
    static void Task5Benchmarks()
    {
        Console.WriteLine("--- ЗАВДАННЯ 5: Бенчмарки часу виконання ---");
        Console.WriteLine("Будь ласка, запустіть збірку в режимі RELEASE для коректних результатів!\n");

        int n = 100_000;
        Stopwatch sw = new Stopwatch();

        // Прогрівальний прогін (на 1000 елементів) за правилами
        List<int> warmup = new List<int>();
        for (int i = 0; i < 1000; i++) warmup.Add(i);

        // 1. 100 000 x Add(x) без місткості
        sw.Restart();
        List<int> list1 = new List<int>();
        for (int i = 0; i < n; i++) list1.Add(i);
        sw.Stop();
        long t1 = sw.ElapsedMilliseconds;

        // 2. 100 000 x Add(x) з місткостю 100 000
        sw.Restart();
        List<int> list2 = new List<int>(n);
        for (int i = 0; i < n; i++) list2.Add(i);
        sw.Stop();
        long t2 = sw.ElapsedMilliseconds;

        // 3. 100 000 x Insert(0, x) (Увага: може зайняти час)
        sw.Restart();
        List<int> list3 = new List<int>();
        for (int i = 0; i < n; i++) list3.Insert(0, i);
        sw.Stop();
        long t3 = sw.ElapsedMilliseconds;

        // 4. 1 000 x Contains(значення, якого немає) у списку з 100 000 елементів
        sw.Restart();
        for (int i = 0; i < 1000; i++)
        {
            list2.Contains(-999999);
        }
        sw.Stop();
        long t4 = sw.ElapsedMilliseconds;

        Console.WriteLine($"1. Add без місткості ({n} елементів):     {t1} мс");
        Console.WriteLine($"2. Add з місткостю ({n} елементів):      {t2} мс");
        Console.WriteLine($"3. Insert(0, x) ({n} елементів):         {t3} мс");
        Console.WriteLine($"4. 1000 x Contains (у 100k списку):        {t4} мс");
    }

    // ==========================================
    // ЗАВДАННЯ 6: Перевірка крайових випадків
    // ==========================================
    static void Task6EdgeCases()
    {
        Console.WriteLine("--- ЗАВДАННЯ 6: Тестування крайових випадків ---\n");

        // 1. Порожній список
        List<int> empty = new List<int>();
        Console.WriteLine("Порожній список -> Завд. 1 (фільтр): " + PrintList(Task1Filter(empty, 3)));
        
        // 2. Одноелементний список
        List<int> single = new List<int> { 5 };
        Console.WriteLine("Одноелементний [5] -> Завд. 1 (фільтр кратних 3): " + PrintList(Task1Filter(single, 3)));

        // 3. Жоден елемент не підходить
        List<int> none = new List<int> { 1, 2, 4, 5 };
        Console.WriteLine("Жоден не підходить [1,2,4,5] (кратні 3): " + PrintList(Task1Filter(none, 3)));

        // 4. Усі елементи підходять
        List<int> all = new List<int> { 3, 6, 9 };
        Console.WriteLine("Усі підходять [3,6,9] (кратні 3): " + PrintList(Task1Filter(all, 3)));
        
        Console.WriteLine("\nКрайові випадки оброблено успішно без аварійних завершень.");
    }
}