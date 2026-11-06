namespace Playfair;

public class PlayfairCipher
{
    private const string Alphabet = "AĂÂBCDEFGHIÎKLMNOPQRSȘTȚUVWXYZ";
    private const int Rows = 6;
    private const int Columns = 5;

    private readonly char[,] matrix = new char[Rows, Columns];

    public PlayfairCipher(string keyword)
    {
        ArgumentNullException.ThrowIfNull(keyword);

        if (keyword.Contains(' '))
        {
            throw new ArgumentException("The keyword must contain letters only, without spaces.");
        }

        keyword = Normalize(keyword);

        if (keyword.Length < 7)
        {
            throw new ArgumentException("The keyword must contain at least 7 letters before removing duplicates.");
        }

        Console.WriteLine("\nKey: " + keyword);
        BuildMatrix(keyword);
    }

    public string Encrypt(string message)
    {
        message = Normalize(message);

        if (message.Length == 0)
        {
            throw new ArgumentException("The message must contain at least one Romanian letter.");
        }

        Console.WriteLine("\nNormalized: " + message);
        List<string> pairs = PreparePairs(message);
        Console.WriteLine("Pairs: " + string.Join(" ", pairs));

        string result = "";

        foreach (string pair in pairs)
        {
            result += TransformPair(pair, 1);
        }

        return result;
    }

    public string Decrypt(string ciphertext)
    {
        ciphertext = Normalize(ciphertext);

        if (ciphertext.Length == 0 || ciphertext.Length % 2 != 0)
        {
            throw new ArgumentException("Ciphertext must contain a nonzero, even number of letters.");
        }

        Console.WriteLine("\nNormalized: " + ciphertext);
        List<string> pairs = new List<string>();

        // Ciphertext is split directly into pairs, without adding fillers.
        for (int index = 0; index < ciphertext.Length; index += 2)
        {
            pairs.Add(ciphertext.Substring(index, 2));
        }

        Console.WriteLine("Pairs: " + string.Join(" ", pairs));
        string result = "";

        foreach (string pair in pairs)
        {
            result += TransformPair(pair, -1);
        }

        return result;
    }

    public void PrintMatrix()
    {
        Console.WriteLine("\nMatrix (I/J share a cell; Î is separate):");

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                char letter = matrix[row, column];
                string label = letter == 'I' ? "I/J" : letter.ToString();
                Console.Write($"{label,-5}");
            }

            Console.WriteLine();
        }
    }

    private static string Normalize(string text)
    {
        ArgumentNullException.ThrowIfNull(text);

        text = text.Replace('ş', 'ș')
            .Replace('Ş', 'Ș')
            .Replace('ţ', 'ț')
            .Replace('Ţ', 'Ț')
            .ToUpperInvariant()
            .Replace('J', 'I')
            .Replace(" ", "");

        foreach (char letter in text)
        {
            if (!Alphabet.Contains(letter))
            {
                throw new ArgumentException(
                    $"Character '{letter}' is not allowed. Use Romanian letters "
                        + "A-Z, Ă, Â, Î, Ș, Ț (uppercase or lowercase). "
                        + "Ordinary spaces are allowed in messages."
                );
            }
        }

        return text;
    }

    private void BuildMatrix(string keyword)
    {
        string letters = "";

        foreach (char letter in keyword)
        {
            if (!letters.Contains(letter))
            {
                letters += letter;
            }
        }

        Console.WriteLine("Unique key: " + letters);

        foreach (char letter in Alphabet)
        {
            if (!letters.Contains(letter))
            {
                letters += letter;
            }
        }

        int index = 0;

        for (int row = 0; row < Rows; row++)
        {
            for (int column = 0; column < Columns; column++)
            {
                matrix[row, column] = letters[index];
                index++;
            }
        }
    }

    private static List<string> PreparePairs(string message)
    {
        List<string> pairs = new List<string>();
        int index = 0;

        while (index < message.Length)
        {
            char first = message[index];
            char second;
            char filler = first == 'X' ? 'Q' : 'X'; // to avoid XX, so, it will be eitgher AX, or XQ

            if (index + 1 == message.Length)
            {
                second = filler;
                Console.WriteLine($"Add {filler} after the final {first}.");
                index++;
            }
            else if (first == message[index + 1])
            {
                second = filler;
                Console.WriteLine($"Insert {filler} between repeated {first} letters.");
                index++; // The second original letter starts the next pair.
            }
            else
            {
                second = message[index + 1];
                index += 2;
            }

            pairs.Add(first.ToString() + second);
        }

        return pairs;
    }

    private string TransformPair(string pair, int direction)
    {
        FindPosition(pair[0], out int firstRow, out int firstColumn);
        FindPosition(pair[1], out int secondRow, out int secondColumn);
        string rule;

        if (firstRow == secondRow)
        {
            firstColumn = (firstColumn + direction + Columns) % Columns;
            secondColumn = (secondColumn + direction + Columns) % Columns;
            rule = "same row";
        }
        else if (firstColumn == secondColumn)
        {
            firstRow = (firstRow + direction + Rows) % Rows;
            secondRow = (secondRow + direction + Rows) % Rows;
            rule = "same column";
        }
        else
        {
            int savedColumn = firstColumn;
            firstColumn = secondColumn;
            secondColumn = savedColumn;
            rule = "rectangle";
        }

        string result = matrix[firstRow, firstColumn].ToString() + matrix[secondRow, secondColumn];
        Console.WriteLine($"{pair} -> {result}  {rule}");
        return result;
    }

    private void FindPosition(char letter, out int row, out int column)
    {
        for (int currentRow = 0; currentRow < Rows; currentRow++)
        {
            for (int currentColumn = 0; currentColumn < Columns; currentColumn++)
            {
                if (matrix[currentRow, currentColumn] == letter)
                {
                    row = currentRow;
                    column = currentColumn;
                    return;
                }
            }
        }

        throw new ArgumentException($"Letter '{letter}' is not in the matrix.");
    }
}
