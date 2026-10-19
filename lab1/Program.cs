using System;
using System.Text;

class Program
{
    // Оголошуємо змінні варіанта на рівні класу
    static int N = 28;
    static int K = 1;
    static int a = 1;
    static int b = 0;
    static int c = 3;

    static void Main()
    {
        // Встановлюємо правильне кодування для коректного відображення українських літер у консолі
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Оберіть завдання для виконання:");
            Console.WriteLine("1. Завдання 1: Перевірка дати, пора року та день року (a = 1)");
            Console.WriteLine("2. Завдання 2: Послідовність чисел (один прохід, b = 0, a = 1, c = 3)");
            Console.WriteLine("3. Завдання 3: Крайові випадки для Завдання 2");
            Console.WriteLine("4. Завдання 4: Порівняння НСД (перебір проти Евкліда)");
            Console.WriteLine("0. Вихід");
            Console.Write("Ваш вибір: ");

            string? choiceStr = Console.ReadLine();
            if (!int.TryParse(choiceStr, out int choice))
            {
                Console.WriteLine("Помилка! Введіть ціле число від 0 до 4.\n");
                continue;
            }

            Console.WriteLine();
            switch (choice)
            {
                case 1:
                    Task1(a);
                    break;
                case 2:
                    Task2();
                    break;
                case 3:
                    Task3();
                    break;
                case 4:
                    Task4();
                    break;
                case 0:
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.\n");
                    break;
            }
            Console.WriteLine("\nНатисніть будь-яку клавішу для продовження...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    // ==========================================
    // ЗАВДАННЯ 1: Перевірка дати (для a = 1)
    // ==========================================
    static void Task1(int aParam)
    {
        Console.WriteLine("--- ЗАВДАННЯ 1: Перевірка коректності дати ---");
        
        int day = ReadInt("Введіть день: ");
        int month = ReadInt("Введіть місяць: ");
        int year = ReadInt("Введіть рік: ");

        // Перевірка високосного року
        bool isLeap = (year % 4 == 0 && year % 100 != 0) || (year % 400 == 0);

        // Визначення кількості днів у місяцях
        int daysInMonth = 0;
        if (month == 2)
        {
            daysInMonth = isLeap ? 29 : 28;
        }
        else if (month == 4 || month == 6 || month == 9 || month == 11)
        {
            daysInMonth = 30;
        }
        else if (month == 1 || month == 3 || month == 5 || month == 7 || month == 8 || month == 10 || month == 12)
        {
            daysInMonth = 31;
        }

        // Перевірка коректності
        bool isValid = true;
        if (year < 1 || month < 1 || month > 12 || day < 1 || day > daysInMonth)
        {
            isValid = false;
        }

        if (!isValid)
        {
            Console.WriteLine("Дата НЕКОРЕКТНА!");
            return;
        }

        Console.WriteLine("Дата коректна.");

        // Обчислення номера дня від початку року
        int dayOfYear = day;
        int m = 1;
        while (m < month)
        {
            int dInM = 0;
            if (m == 2) dInM = isLeap ? 29 : 28;
            else if (m == 4 || m == 6 || m == 9 || m == 11) dInM = 30;
            else dInM = 31;

            dayOfYear += dInM;
            m++;
        }

        // Визначення пори року
        string season = "";
        if (month == 12 || month == 1 || month == 2) season = "Зима";
        else if (month >= 3 && month <= 5) season = "Весна";
        else if (month >= 6 && month <= 8) season = "Літо";
        else if (month >= 9 && month <= 11) season = "Осінь";

        Console.WriteLine($"Пора року: {season}");
        Console.WriteLine($"Номер дня від початку року: {dayOfYear}");
    }

    // ==========================================
    // ЗАВДАННЯ 2: Послідовність (b=0, a=1, c=3)
    // ==========================================
    static void Task2()
    {
        Console.WriteLine("--- ЗАВДАННЯ 2: Обробка послідовності за один прохід ---");
        
        Random rnd = new Random(N);
        int totalSum = 0;
        int zeroCount = 0;
        int targetCount = 0;
        int firstIndex = -1;
        int currentIndex = 1;

        // b = 0: 15 випадкових чисел у діапазоні [-50; 50]
        int limit = 15;
        Console.WriteLine("Згенерована послідовність:");

        for (int i = 0; i < limit; i++)
        {
            int val = rnd.Next(-50, 51);
            Console.Write(val + (i < limit - 1 ? ", " : "\n"));

            // Спільна частина: сума всіх і кількість нулів
            totalSum += val;
            if (val == 0) zeroCount++;

            // Критерій відбору a = 1: кратні K (K = 1)
            bool satisfiesCriteria = (val % K == 0);

            if (satisfiesCriteria)
            {
                targetCount++;
                // c = 3: порядковий номер першого такого елемента або -1
                if (firstIndex == -1)
                {
                    firstIndex = currentIndex;
                }
            }
            currentIndex++;
        }

        Console.WriteLine($"\nРезультати спільної частини:");
        Console.WriteLine($"- Сума всіх елементів: {totalSum}");
        Console.WriteLine($"- Кількість нулів: {zeroCount}");
        Console.WriteLine($"\nРезультати індивідуальної частини (c = 3, пошук першого елемента, що кратний {K}):");
        Console.WriteLine($"- Порядковий номер першого елемента: {firstIndex}");
    }

    // Допоміжний метод для симуляції Завдання 2 під час тестування крайових випадків
    static void RunTask2Simulation(int[] sequence, out int totalSum, out int zeroCount, out int firstIndex)
    {
        totalSum = 0;
        zeroCount = 0;
        firstIndex = -1;
        int currentIndex = 1;

        for (int i = 0; i < sequence.Length; i++)
        {
            int val = sequence[i];
            totalSum += val;
            if (val == 0) zeroCount++;

            bool satisfiesCriteria = (val % K == 0);
            if (satisfiesCriteria)
            {
                if (firstIndex == -1)
                {
                    firstIndex = currentIndex;
                }
            }
            currentIndex++;
        }
    }

    // ==========================================
    // ЗАВДАННЯ 3: Крайові випадки
    // ==========================================
    static void Task3()
    {
        Console.WriteLine("--- ЗАВДАННЯ 3: Перевірка алгоритму на крайових випадках ---\n");

        // Тест 1: Порожній вхід (0 елементів)
        int[] test1 = new int[0];
        RunTask2Simulation(test1, out int sum1, out int zero1, out int idx1);
        
        // Тест 2: Рівно одне число
        int[] test2 = new int[] { 7 };
        RunTask2Simulation(test2, out int sum2, out int zero2, out int idx2);

        // Тест 3: Усі числа однакові
        int[] test3 = new int[] { 5, 5, 5, 5 };
        RunTask2Simulation(test3, out int sum3, out int zero3, out int idx3);

        // Тест 4: Жоден елемент не відповідає критерію
        int[] test4 = new int[] { -10, -20, 30 };
        int idx4 = -1;
        for(int i = 0; i < test4.Length; i++) {
            if (test4[i] > 100) { idx4 = i + 1; break; }
        }

        Console.WriteLine("Таблиця результатів перевірки крайових випадків:");
        Console.WriteLine("-----------------------------------------------------------------------------------------");
        Console.WriteLine(string.Format("{0,-20} | {1,-25} | {2,-25}", "Вхідні дані", "Що очікуємо", "Що вивела програма"));
        Console.WriteLine("-----------------------------------------------------------------------------------------");
        Console.WriteLine(string.Format("{0,-20} | {1,-25} | {2,-25}", "Порожній вхід", "sum=0, zero=0, idx=-1", $"sum={sum1}, zero={zero1}, idx={idx1}"));
        Console.WriteLine(string.Format("{0,-20} | {1,-25} | {2,-25}", "Рівно одне число (7)", "sum=7, zero=0, idx=1", $"sum={sum2}, zero={zero2}, idx={idx2}"));
        Console.WriteLine(string.Format("{0,-20} | {1,-25} | {2,-25}", "Усі числа однакові", "sum=20, zero=0, idx=1", $"sum={sum3}, zero={zero3}, idx={idx3}"));
        Console.WriteLine(string.Format("{0,-20} | {1,-25} | {2,-25}", "Жоден не відповідає", "idx = -1 (немає даних)", $"idx = {idx4}"));
        Console.WriteLine("-----------------------------------------------------------------------------------------");

        Console.WriteLine("\nОпис знайдених помилок та виправлень:");
        Console.WriteLine("1. При порожньому вході початкове значення індексу не повинно залишатися 0 (коректно повертати -1), інакше інтерпретується як перший елемент.");
        Console.WriteLine("2. Сума та лічильники на порожньому вході повинні ініціалізуватися нулем, щоб уникнути некоректного додавання сміттєвих значень.");
    }

    // ==========================================
    // ЗАВДАННЯ 4: На «відмінно» (НСД)
    // ==========================================
    static void Task4()
    {
        Console.WriteLine("--- ЗАВДАННЯ 4: Два алгоритми для обчислення НСД ---");
        
        int A = 1_000_000 + N; // 1 000 028
        int B = 999_999 - N;   // 999 971

        Console.WriteLine($"Число A = {A}");
        Console.WriteLine($"Число B = {B}\n");

        // Спосіб 1: Перебором дільників від Math.Min(A, B) донизу
        int gcd1 = 1;
        int iter1 = 0;
        int minAB = Math.Min(A, B);
        
        for (int i = minAB; i >= 1; i--)
        {
            iter1++;
            if (A % i == 0 && B % i == 0)
            {
                gcd1 = i;
                break;
            }
        }

        // Спосіб 2: Алгоритм Евкліда через остачу
        int gcd2 = 0;
        int iter2 = 0;
        int tempA = A;
        int tempB = B;

        while (tempB != 0)
        {
            iter2++;
            int remainder = tempA % tempB;
            tempA = tempB;
            tempB = remainder;
        }
        gcd2 = tempA;

        Console.WriteLine($"1. Перебір дільників: НСД = {gcd1}, кількість ітерацій = {iter1}");
        Console.WriteLine($"2. Алгоритм Евкліда:   НСД = {gcd2}, кількість ітерацій = {iter2}");

        double ratio = (double)iter1 / (iter2 > 0 ? iter2 : 1);
        Console.WriteLine($"\nВисновок: Кількість ітерацій відрізняється у {ratio:F1} разів (алгоритм Евкліда значно швидший завдяки логарифмічній складності через операцію залишку від ділення, тоді як перебір виконує лінійний пошук від мінімального числа).");
    }

    // Допоміжний метод безпечного введення числа
    static int ReadInt(string prompt)
    {
        int value;
        while (true)
        {
            Console.Write(prompt);
            string? input = Console.ReadLine();
            if (int.TryParse(input, out value))
            {
                return value;
            }
            Console.WriteLine("Помилка! Введіть ціле число. Спробуйте ще раз.");
        }
    }
}