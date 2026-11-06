using System.Text;

namespace Playfair;

internal class Program
{
    static void Main()
    {
        Console.InputEncoding = Encoding.UTF8;
        Console.OutputEncoding = Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n=== Romanian Playfair ===");
            Console.WriteLine("1. Encrypt");
            Console.WriteLine("2. Decrypt");
            Console.WriteLine("0. Exit");

            int operation = ReadChoice();

            if (operation == 0)
            {
                return;
            }

            PlayfairCipher cipher;

            while (true)
            {
                Console.Write("Keyword (at least 7 letters, no spaces): ");
                string? keyword = Console.ReadLine();

                if (keyword == null)
                {
                    return;
                }

                try
                {
                    cipher = new PlayfairCipher(keyword);
                    break;
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }

            cipher.PrintMatrix();

            while (true)
            {
                Console.Write(operation == 1 ? "\nMessage: " : "\nCiphertext: ");
                string? text = Console.ReadLine();

                if (text == null)
                {
                    return;
                }

                try
                {
                    if (operation == 1)
                    {
                        string result = cipher.Encrypt(text);
                        Console.WriteLine("\nCiphertext: " + result);
                    }
                    else
                    {
                        string result = cipher.Decrypt(text);
                        Console.WriteLine("\nDecrypted text: " + result);
                        Console.WriteLine("Remove inserted fillers and restore spaces manually; I/J share a cell.");
                    }

                    break;
                }
                catch (ArgumentException exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
    }

    private static int ReadChoice()
    {
        while (true)
        {
            Console.Write("Choose operation: ");
            string? input = Console.ReadLine();

            if (input == null)
            {
                return 0;
            }

            if (int.TryParse(input, out int choice) && choice >= 0 && choice <= 2)
            {
                return choice;
            }

            Console.WriteLine("Enter 1 to encrypt, 2 to decrypt, or 0 to exit.");
        }
    }
}
