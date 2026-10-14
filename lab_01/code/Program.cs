using System;

// 2 chyphers
// encrypt/decrypt
// method of encryption
// recieve message and transform
// handle romanian alphabet
// use numbers for calculation.

class CeasarChipher
{
    public string romanian_alphabet = "aăâbcdefghiîjklmnopqrsștțuvwxyz"; // [0] = a, [1] = ă, ...

    public string Normalize(string message)
    {
        return message.Replace('ş', 'ș').Replace('Ş', 'Ș').Replace('ţ', 'ț').Replace('Ţ', 'Ț');
    }

    public string encrypt(string message, int key)
    {
        if (!((key >= 1) && (key <= 30)))
        {
            throw new ArgumentException($"Provided key '{key}' should be between 1 and 30.");
        }
        message = Normalize(message);
        Validate(message);
        message = message.Replace(" ", "").ToUpperInvariant(); // remove spaces and transform to uppercase

        int[] message_in_numbers = new int[message.Length];
        for (int i = 0; i < message.Length; i++)
        {
            message_in_numbers[i] = getNumericValueOfLetter(message[i]);
        }

        // the actual encryption

        int[] encrypted_message_in_numbers = new int[message.Length];
        for (int i = 0; i < message.Length; i++)
        {
            encrypted_message_in_numbers[i] = (message_in_numbers[i] + key) % 31; // 31 is the size of romanian_alphabet
        }

        // transform back to string

        string encrypted = "";
        foreach (int numeric_code in encrypted_message_in_numbers)
        {
            encrypted += getLetterByNumericValue(numeric_code);
        }

        return encrypted;
    }

    public int getNumericValueOfLetter(char c)
    {
        for (int i = 1; i <= 31; i++)
        {
            if (romanian_alphabet[i - 1] == c)
            {
                return i;
            }
        }
        throw new Exception(
            "Somehow the letter is not convertable.. it is before a or after z or somehow like that"
        );
    }

    public char getLetterByNumericValue(int value)
    {
        return romanian_alphabet[value - 1];
    }

    public void Validate(string message)
    {
        string allowedLetters = "aăâbcdefghiîjklmnopqrsștțuvwxyz ";

        foreach (char letter in message.ToLowerInvariant())
        {
            if (!allowedLetters.Contains(letter))
            {
                throw new ArgumentException(
                    $"Character '{letter}' is not allowed. Use romanian alphabet"
                );
            }
        }
    }
}

class Program
{
    static void Main(string[] args)
    {
        var chip = new CeasarChipher();
        var m = chip.encrypt("hello", 3);
        Console.WriteLine(m);
    }
}
