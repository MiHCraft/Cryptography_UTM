using System;

// ============================================================
// Romanian Alphabet
// Shared alphabet operations for both cipher implementations
// ============================================================

static class RomanianAlphabet
{
    public const string Letters = "AĂÂBCDEFGHIÎJKLMNOPQRSȘTȚUVWXYZ";
    public const int Size = 31;

    public static string Normalize(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        return text.Replace('ş', 'ș')
            .Replace('Ş', 'Ș')
            .Replace('ţ', 'ț')
            .Replace('Ţ', 'Ț')
            .ToUpperInvariant();
    }

    public static void Validate(string text, bool allowSpaces = true)
    {
        foreach (char letter in text)
        {
            if (allowSpaces && letter == ' ')
                continue;

            if (!Letters.Contains(letter))
            {
                throw new ArgumentException(
                    $"Character '{letter}' is not allowed. "
                        + $"Allowed letters: {Letters}"
                        + (allowSpaces ? " and spaces." : ".")
                );
            }
        }
    }

    public static string Prepare(string text)
    {
        text = Normalize(text);
        Validate(text);

        return text.Replace(" ", "");
    }

    public static string PrepareKeyword(string keyword)
    {
        keyword = Normalize(keyword);
        Validate(keyword, allowSpaces: false);

        if (keyword.Length < 7)
        {
            throw new ArgumentException("Keyword must contain at least 7 letters.");
        }

        return keyword;
    }

    public static void ValidateKey(int key)
    {
        if (key < 1 || key > 30)
        {
            throw new ArgumentException("Numeric key must be an integer between 1 and 30.");
        }
    }

    public static int[] ToNumbers(string text, string alphabet)
    {
        int[] numbers = new int[text.Length];

        for (int i = 0; i < text.Length; i++)
        {
            int index = alphabet.IndexOf(text[i]);

            if (index < 0)
            {
                throw new ArgumentException($"Character '{text[i]}' is not in the alphabet.");
            }

            numbers[i] = index;
        }

        return numbers;
    }

    public static string ToText(int[] numbers, string alphabet)
    {
        char[] result = new char[numbers.Length];

        for (int i = 0; i < numbers.Length; i++)
        {
            result[i] = alphabet[numbers[i]];
        }

        return new string(result);
    }

    public static int[] ShiftNumbers(int[] numbers, int key)
    {
        int[] shifted = new int[numbers.Length];

        for (int i = 0; i < numbers.Length; i++)
        {
            shifted[i] = (numbers[i] + key + Size) % Size;
        }

        return shifted;
    }
}

// ============================================================
// Task 1.1 - Standard Caesar Cipher
// ============================================================

class CaesarCipher
{
    public string Encrypt(string message, int key)
    {
        RomanianAlphabet.ValidateKey(key);
        message = RomanianAlphabet.Prepare(message);

        int[] numbers = RomanianAlphabet.ToNumbers(message, RomanianAlphabet.Letters);

        int[] encrypted = RomanianAlphabet.ShiftNumbers(numbers, key);

        return RomanianAlphabet.ToText(encrypted, RomanianAlphabet.Letters);
    }

    public string Decrypt(string message, int key)
    {
        RomanianAlphabet.ValidateKey(key);
        message = RomanianAlphabet.Prepare(message);

        int[] numbers = RomanianAlphabet.ToNumbers(message, RomanianAlphabet.Letters);

        int[] decrypted = RomanianAlphabet.ShiftNumbers(numbers, -key);

        return RomanianAlphabet.ToText(decrypted, RomanianAlphabet.Letters);
    }

    public void PrintEncryptionTable(int key)
    {
        RomanianAlphabet.ValidateKey(key);

        CipherTablePrinter.Print(RomanianAlphabet.Letters, key);
    }
}

// ============================================================
// Task 1.2 - Caesar Cipher with Two Keys
// ============================================================

class TwoKeyCaesarCipher
{
    public string Encrypt(string message, int key, string keyword)
    {
        RomanianAlphabet.ValidateKey(key);
        message = RomanianAlphabet.Prepare(message);

        string alphabet = BuildPermutedAlphabet(keyword);

        int[] numbers = RomanianAlphabet.ToNumbers(message, alphabet);
        int[] encrypted = RomanianAlphabet.ShiftNumbers(numbers, key);

        return RomanianAlphabet.ToText(encrypted, alphabet);
    }

    public string Decrypt(string message, int key, string keyword)
    {
        RomanianAlphabet.ValidateKey(key);
        message = RomanianAlphabet.Prepare(message);

        string alphabet = BuildPermutedAlphabet(keyword);

        int[] numbers = RomanianAlphabet.ToNumbers(message, alphabet);
        int[] decrypted = RomanianAlphabet.ShiftNumbers(numbers, -key);

        return RomanianAlphabet.ToText(decrypted, alphabet);
    }

    public string BuildPermutedAlphabet(string keyword)
    {
        keyword = RomanianAlphabet.PrepareKeyword(keyword);

        string result = "";

        // Add unique letters from the keyword
        foreach (char letter in keyword)
        {
            if (!result.Contains(letter))
            {
                result += letter;
            }
        }

        // Append remaining Romanian alphabet letters
        foreach (char letter in RomanianAlphabet.Letters)
        {
            if (!result.Contains(letter))
            {
                result += letter;
            }
        }

        return result;
    }

    public void PrintEncryptionTable(int key, string keyword)
    {
        RomanianAlphabet.ValidateKey(key);

        string alphabet = BuildPermutedAlphabet(keyword);

        Console.WriteLine($"\nKeyword: {RomanianAlphabet.Normalize(keyword)}");
        Console.WriteLine($"Original alphabet: {RomanianAlphabet.Letters}");
        Console.WriteLine($"Permuted alphabet: {alphabet}");

        CipherTablePrinter.Print(alphabet, key);
    }
}

// ============================================================
// Cipher Table Printer
// Displays numeric and letter mappings for manual verification
// ============================================================

static class CipherTablePrinter
{
    public static void Print(string alphabet, int key)
    {
        int[] original = new int[alphabet.Length];

        for (int i = 0; i < original.Length; i++)
        {
            original[i] = i;
        }

        int[] shifted = RomanianAlphabet.ShiftNumbers(original, key);

        Console.WriteLine($"\nEncryption Table (key = {key})\n");

        PrintRow("x", original);
        PrintRow("m", original, alphabet);
        PrintRow("x+k", shifted);
        PrintRow("c", shifted, alphabet);

        Console.WriteLine();
    }

    private static void PrintRow(string label, int[] numbers, string? alphabet = null)
    {
        Console.Write($"{label, -4}|");

        foreach (int number in numbers)
        {
            string value = alphabet == null ? number.ToString() : alphabet[number].ToString();

            Console.Write($"{value, 3}|");
        }

        Console.WriteLine();
    }
}

// ============================================================
// Program - Console Interface
// ============================================================

class Program
{
    static void Main()
    {
        var caesar = new CaesarCipher();
        var twoKeyCaesar = new TwoKeyCaesarCipher();

        while (true)
        {
            Console.WriteLine("\n=== Romanian Caesar Cipher ===");
            Console.WriteLine("1. Standard Caesar Cipher");
            Console.WriteLine("2. Two-Key Caesar Cipher");
            Console.WriteLine("0. Exit");

            int cipherType = ReadChoice("Choose cipher: ", 0, 2);

            if (cipherType == 0)
                break;

            Console.WriteLine("\n1. Encrypt");
            Console.WriteLine("2. Decrypt");

            int operation = ReadChoice("Choose operation: ", 1, 2);

            int key = ReadNumericKey();

            string keyword = "";

            if (cipherType == 2)
            {
                keyword = ReadKeyword();
            }

            string message = ReadMessage();
            string result;

            if (cipherType == 1)
            {
                caesar.PrintEncryptionTable(key);

                result =
                    operation == 1 ? caesar.Encrypt(message, key) : caesar.Decrypt(message, key);
            }
            else
            {
                twoKeyCaesar.PrintEncryptionTable(key, keyword);

                result =
                    operation == 1
                        ? twoKeyCaesar.Encrypt(message, key, keyword)
                        : twoKeyCaesar.Decrypt(message, key, keyword);
            }

            Console.WriteLine($"Input:  {message}");
            Console.WriteLine($"Result: {result}");
        }
    }

    private static int ReadChoice(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);

            string? input = Console.ReadLine();

            if (int.TryParse(input, out int choice) && choice >= min && choice <= max)
            {
                return choice;
            }

            Console.WriteLine($"Invalid choice. Enter a number from {min} to {max}.");
        }
    }

    private static int ReadNumericKey()
    {
        while (true)
        {
            Console.Write("Enter numeric key (1-30): ");

            string? input = Console.ReadLine();

            if (int.TryParse(input, out int key) && key >= 1 && key <= 30)
            {
                return key;
            }

            Console.WriteLine("Invalid key. Enter an integer between 1 and 30.");
        }
    }

    private static string ReadKeyword()
    {
        while (true)
        {
            Console.Write("Enter keyword (at least 7 letters): ");

            string keyword = Console.ReadLine() ?? "";

            try
            {
                return RomanianAlphabet.PrepareKeyword(keyword);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid keyword: {ex.Message}");
            }
        }
    }

    private static string ReadMessage()
    {
        while (true)
        {
            Console.Write("Enter message: ");

            string message = Console.ReadLine() ?? "";

            try
            {
                return RomanianAlphabet.Prepare(message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Invalid message: {ex.Message}");
            }
        }
    }
}



