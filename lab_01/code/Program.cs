using System;

class CaesarCipher
{
    private readonly string alphabet = "aăâbcdefghiîjklmnopqrsștțuvwxyz".ToUpperInvariant();

    public string Encrypt(string message, int key)
    {
        ValidateKey(key);
        message = PrepareMessage(message);

        int[] numbers = ConvertToNumbers(message);
        int[] encryptedNumbers = ShiftNumbers(numbers, key);

        return ConvertToString(encryptedNumbers);
    }

    public string Decrypt(string message, int key)
    {
        ValidateKey(key);
        message = PrepareMessage(message);

        int[] numbers = ConvertToNumbers(message);
        int[] decryptedNumbers = ShiftNumbers(numbers, -key);

        return ConvertToString(decryptedNumbers);
    }

    private string PrepareMessage(string message)
    {
        message = Normalize(message);
        Validate(message);

        return message.Replace(" ", "").ToUpperInvariant();
    }

    private string Normalize(string message)
    {
        return message.Replace('ş', 'ș').Replace('Ş', 'Ș').Replace('ţ', 'ț').Replace('Ţ', 'Ț');
    }

    private void Validate(string message)
    {
        foreach (char letter in message.ToUpperInvariant())
        {
            if (letter != ' ' && !alphabet.Contains(letter))
            {
                throw new ArgumentException($"Character '{letter}' is not allowed.");
            }
        }
    }

    private void ValidateKey(int key)
    {
        if (key < 1 || key >= alphabet.Length)
        {
            throw new ArgumentException($"Key must be between 1 and {alphabet.Length - 1}.");
        }
    }

    private int[] ConvertToNumbers(string message)
    {
        int[] numbers = new int[message.Length];

        for (int i = 0; i < message.Length; i++)
        {
            numbers[i] = alphabet.IndexOf(message[i]);
        }

        return numbers;
    }

    private int[] ShiftNumbers(int[] numbers, int key)
    {
        int[] shifted = new int[numbers.Length];

        for (int i = 0; i < numbers.Length; i++)
        {
            shifted[i] = ((numbers[i] + key) % alphabet.Length + alphabet.Length) % alphabet.Length;
        }

        return shifted;
    }

    private string ConvertToString(int[] numbers)
    {
        char[] result = new char[numbers.Length];

        for (int i = 0; i < numbers.Length; i++)
        {
            result[i] = alphabet[numbers[i]];
        }

        return new string(result);
    }

    public void PrintEncryptionTable(int key)
    {
        ValidateKey(key);

        int length = alphabet.Length;

        int[] originalNumbers = new int[length];

        for (int i = 0; i < length; i++)
        {
            originalNumbers[i] = i;
        }

        int[] shiftedNumbers = ShiftNumbers(originalNumbers, key);

        Console.WriteLine($"\nCaesar Cipher Table (key = {key})\n");

        // Row 1: Original numeric values
        Console.Write("x   |");
        foreach (int number in originalNumbers)
            Console.Write($"{number, 3}|");

        Console.WriteLine();

        // Row 2: Original letters
        Console.Write("m   |");
        foreach (int number in originalNumbers)
            Console.Write($"  {alphabet[number]}|");

        Console.WriteLine();

        // Row 3: Shifted numeric values
        Console.Write("x+k |");
        foreach (int number in shiftedNumbers)
            Console.Write($"{number, 3}|");

        Console.WriteLine();

        // Row 4: Encrypted letters
        Console.Write("c   |");
        foreach (int number in shiftedNumbers)
            Console.Write($"  {alphabet[number]}|");

        Console.WriteLine();
    }
}

class Program
{
    static void Main()
    {
        var cipher = new CaesarCipher();

        string original = "hello";
        int key = 3;

        cipher.PrintEncryptionTable(key);

        string encrypted = cipher.Encrypt(original, key);
        string decrypted = cipher.Decrypt(encrypted, key);

        Console.WriteLine($"Original:  {original}");
        Console.WriteLine($"Encrypted: {encrypted}");
        Console.WriteLine($"Decrypted: {decrypted}");
    }
}
