using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    // Параметри варіанта
    static int N = 28;
    static int K = 1;
    static int a = 1; // Завдання 1: дужки ( ) [ ] { } < >, індекс першої помилки
    static int b = 0; // Завдання 2: операції + - * /
    static int c = 3; // Завдання 3: 9 заявок, два обслуговувачі з однієї спільної черги

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("Оберіть завдання для виконання:");
            Console.WriteLine("1. Завдання 1: Перевірка збалансованості дужок (усі види + <>)");
            Console.WriteLine("2. Завдання 2: Обчислення виразу у зворотному польському записі (ЗПЗ)");
            Console.WriteLine("3. Завдання 3: Модель обслуговування (9 заявок, 2 обслуговувачі)");
            Console.WriteLine("4. Завдання 4: Власний клас IntStack поверх масиву з подвоєнням");
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
                    RunTask1Tests();
                    break;
                case 2:
                    RunTask2Tests();
                    break;
                case 3:
                    RunTask3Model();
                    break;
                case 4:
                    RunTask4StackDemo();
                    break;
                case 6:
                    RunTask6EdgeCases();
                    break;
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

    // ==========================================
    // ЗАВДАННЯ 1: Збалансованість дужок
    // ==========================================
    static (bool isValid, int errorIndex) CheckBrackets(string input)
    {
        if (string.IsNullOrEmpty(input)) return (true, -1);

        Stack<char> stack = new Stack<char>();
        Stack<int> indexStack = new Stack<int>();

        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];

            if (c == '(' || c == '[' || c == '{' || c == '<')
            {
                stack.Push(c);
                indexStack.Push(i);
            }
            else if (c == ')' || c == ']' || c == '}' || c == '>')
            {
                // Закривальна дужка при порожньому стеку
                if (stack.Count == 0)
                {
                    return (false, i);
                }

                char lastOpen = stack.Pop();
                indexStack.Pop();

                if (!IsMatchingPair(lastOpen, c))
                {
                    return (false, i);
                }
            }
        }

        // Непорожній стек після кінця рядка
        if (stack.Count > 0)
        {
            // Індекс першої незакритої дужки залишається на вершині indexStack
            int firstUnclosedIndex = -1;
            while (indexStack.Count > 0)
            {
                firstUnclosedIndex = indexStack.Pop();
            }
            return (false, firstUnclosedIndex);
        }

        return (true, -1);
    }

    static bool IsMatchingPair(char open, char close)
    {
        return (open == '(' && close == ')') ||
               (open == '[' && close == ']') ||
               (open == '{' && close == '}') ||
               (open == '<' && close == '>');
    }

    static void RunTask1Tests()
    {
        Console.WriteLine("--- ЗАВДАННЯ 1: Перевірка дужок ---");
        string[] testStrings = {
            "(a[b]{c})",
            "([)]",
            ")(",
            "((",
            "",
            "(((" // Рівно K (1) відкривальна і K-1 (0) закривальних -> свідомо незбалансований
        };

        foreach (var test in testStrings)
        {
            var (isValid, errIdx) = CheckBrackets(test);
            Console.WriteLine($"Рядок: \"{test}\" => Збалансований: {isValid}, Індекс помилки: {errIdx}");
        }
    }

    // ==========================================
    // ЗАВДАННЯ 2: Зворотний польський запис (ЗПЗ)
    // ==========================================
    static double EvaluateRPN(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            throw new ArgumentException("Вираз порожній.");

        string[] tokens = expression.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        Stack<double> stack = new Stack<double>();

        foreach (string token in tokens)
        {
            if (double.TryParse(token, out double number))
            {
                stack.Push(number);
            }
            else if (token == "+" || token == "-" || token == "*" || token == "/")
            {
                if (stack.Count < 2)
                    throw new InvalidOperationException($"Недостатньо операндів для операції '{token}'.");

                double b = stack.Pop();
                double a = stack.Pop();
                double res = 0;

                switch (token)
                {
                    case "+": res = a + b; break;
                    case "-": res = a - b; break;
                    case "*": res = a * b; break;
                    case "/":
                        if (b == 0) throw new DivideByZeroException("Помилка: ділення на нуль!");
                        res = a / b;
                        break;
                }
                stack.Push(res);
            }
            else
            {
                throw new FormatException($"Невідомий токен: '{token}'.");
            }
        }

        if (stack.Count != 1)
            throw new InvalidOperationException("Некоректний вираз: забагато чисел або незавершені операції.");

        return stack.Pop();
    }

    static void RunTask2Tests()
    {
        Console.WriteLine("--- ЗАВДАННЯ 2: Зворотний польський запис ---");
        string[] rpnTests = {
            "3 4 + 2 *",
            "5 3 -",
            "8 2 /",
            "2 3 4 * +",
            "5 0 /", // Тест на ділення на нуль
            "5 3 2 + *" // Власний вираз із результатом K = 1: (3 + 2) = 5; 5 * 5 = 25... Зробимо інший, щоб результат дорівнював K = 1: наприклад "3 2 - 1 *" -> (3-2)*1 = 1
        };

        foreach (var test in rpnTests)
        {
            try
            {
                double result = EvaluateRPN(test);
                Console.WriteLine($"Вираз: \"{test}\" => Результат: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Вираз: \"{test}\" => [Помилка] {ex.Message}");
            }
        }

        // Власний вираз на K = 1 (результат 1, щонайменше 3 операції)
        // Наприклад: "5 2 - 3 / 2 *" => (5 - 2) = 3; 3 / 3 = 1; 1 * 2 = 2. Зробимо: "4 2 - 2 /" => (4-2)/2 = 1 (операції: -, / - лише дві).
        // Додамо три операції: "10 2 / 2 - 3 *" => 10/2=5; 5-2=3; 3*3=9.
        // Потрібно результат 1: "8 2 / 2 / 2 *" -> 8/2=4; 4/2=2; 2*2=4.
        // Простий варіант із результатом 1: "3 2 - 5 5 - +" -> (3-2)=1, (5-5)=0, 1+0=1. Операції: -, -, +. Три операції!
        string customExpr = "3 2 - 5 5 - +";
        try
        {
            double resCustom = EvaluateRPN(customExpr);
            Console.WriteLine($"\nВласний вираз для K={K}: \"{customExpr}\" => Результат: {resCustom}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Помилка власний вираз]: {ex.Message}");
        }
    }

    // ==========================================
    // ЗАВДАННЯ 3: Модель обслуговування (схема c = 3)
    // ==========================================
    class Request
    {
        public int Id { get; set; }
        public int WorkUnits { get; set; }
        public int StartTime { get; set; }
        public int EndTime { get; set; }
    }

    static void RunTask3Model()
    {
        Console.WriteLine("--- ЗАВДАННЯ 3: Модель обслуговування (два обслуговувачі, спільна черга) ---");
        int totalRequests = 8 + (K % 5); // 8 + 1 = 9 заявок за варіантом
        Random rnd = new Random(N);

        Queue<Request> queue = new Queue<Request>();
        for (int i = 1; i <= totalRequests; i++)
        {
            queue.Enqueue(new Request
            {
                Id = i,
                WorkUnits = rnd.Next(1, 10) // вага від 1 до 9
            });
        }

        int maxQueueLength = queue.Count;
        int currentTime = 0;

        // Два обслуговувачі
        int worker1FreeAt = 0;
        int worker2FreeAt = 0;

        List<Request> completed = new List<Request>();

        while (queue.Count > 0 || currentTime <= Math.Max(worker1FreeAt, worker2FreeAt))
        {
            // Оновлюємо максимальну довжину черги
            if (queue.Count > maxQueueLength) maxQueueLength = queue.Count;

            // Якщо хвилини просуваються, перевіряємо вільних обслуговувачів
            if (queue.Count > 0)
            {
                if (currentTime >= worker1FreeAt)
                {
                    Request req = queue.Dequeue();
                    req.StartTime = currentTime;
                    req.EndTime = currentTime + req.WorkUnits;
                    worker1FreeAt = req.EndTime;
                    completed.Add(req);
                }
                else if (currentTime >= worker2FreeAt)
                {
                    Request req = queue.Dequeue();
                    req.StartTime = currentTime;
                    req.EndTime = currentTime + req.WorkUnits;
                    worker2FreeAt = req.EndTime;
                    completed.Add(req);
                }
                else
                {
                    currentTime++;
                }
            }
            else
            {
                currentTime++;
            }
        }

        // Виводимо результати
        foreach (var r in completed)
        {
            Console.WriteLine($"Заявка #{r.Id}: вага = {r.WorkUnits} од., початок = {r.StartTime} хв, завершення = {r.EndTime} хв");
        }
        Console.WriteLine($"\nМаксимальна довжина черги за час моделювання: {maxQueueLength}");
    }

    // ==========================================
    // ЗАВДАННЯ 4: Власний клас IntStack
    // ==========================================
    class IntStack
    {
        private int[] _data;
        private int _top;

        public IntStack(int initialCapacity)
        {
            _data = new int[initialCapacity];
            _top = 0;
        }

        public int Count => _top;
        public int Capacity => _data.Length;

        public void Push(int item)
        {
            if (_top == _data.Length)
            {
                // Подвоєння масиву
                int[] newаrray = new int[_data.Length * 2];
                for (int i = 0; i < _data.Length; i++)
                {
                    newаrray[i] = _data[i];
                }
                _data = newаrray;
            }
            _data[_top++] = item;
        }

        public int Pop()
        {
            if (_top == 0)
                throw new InvalidOperationException("Стек порожній.");
            return _data[--_top];
        }

        public int Peek()
        {
            if (_top == 0)
                throw new InvalidOperationException("Стек порожній.");
            return _data[_top - 1];
        }
    }

    static void RunTask4StackDemo()
    {
        Console.WriteLine("--- ЗАВДАННЯ 4: Демонстрація власного IntStack ---");
        // Початкова місткість: K % 4 + 2 = 1 % 4 + 2 = 3
        int initialCap = (K % 4) + 2;
        IntStack stack = new IntStack(initialCap);

        // Кількість викликів Push: K % 4 + 3 = 1 % 4 + 3 = 4 виклики
        int pushCount = (K % 4) + 3;

        Console.WriteLine($"Початкова місткість: {stack.Capacity}");
        for (int i = 1; i <= pushCount; i++)
        {
            stack.Push(i * 10);
            Console.WriteLine($"Після Push({i * 10}): Count = {stack.Count}, Capacity = {stack.Capacity}");
        }
    }

    // ==========================================
    // ЗАВДАННЯ 6: Крайові випадки
    // ==========================================
    static void RunTask6EdgeCases()
    {
        Console.WriteLine("--- ЗАВДАННЯ 6: Перевірка крайових випадків ---");

        // 1. Порожній рядок для дужок
        var (isVal, err) = CheckBrackets("");
        Console.WriteLine($"Порожній рядок дужок => Збалансований: {isVal}, помилка: {err}");

        // 2. Рядок без жодної дужки
        var (isVal2, err2) = CheckBrackets("abcde");
        Console.WriteLine($"Рядок без дужок => Збалансований: {isVal2}, помилка: {err2}");

        // 3. Вираз із одного числа в ЗПЗ
        try
        {
            double res = EvaluateRPN("42");
            Console.WriteLine($"ЗПЗ \"42\" => Результат: {res}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ЗПЗ \"42\" => [Помилка] {ex.Message}");
        }

        // 4. Ділення на нуль у ЗПЗ
        try
        {
            EvaluateRPN("10 0 /");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ЗПЗ ділення на нуль => [Помилка оброблена]: {ex.Message}");
        }

        // 5. Pop на порожньому власному стеку
        try
        {
            IntStack emptyStk = new IntStack(2);
            emptyStk.Pop();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Pop на порожньому IntStack => [Виняток перехоплено]: {ex.Message}");
        }
    }
}