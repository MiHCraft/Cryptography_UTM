using task_08;

// .8. Să se calculeze Ri pentru runda k a algoritmului DES, dacă se cunoaște Lk-1 și rezultatul aplicării cutiilor-S.

// să fie afișate la ecran tabelele utilizate și toți pașii intermediari.

// Datele de intrare să fie posibil de introdus de utilizator SAU de generat în mod aleatoriu.

// input: L[k-1], that = S1(B1)S2(B2)S3(B3)S4(B4)S5(B5)S6(B6)S7(B7)S8(B8)
//
// intermediate steps:
// 1. f = P(that) (permutation)
// 2. L[k-1] XOR f => R[k]
//
// output: R[k]
//

uint Ln_minus_one = 0; // L[k-1]
uint sboxes_result = 0; // S1(B1)S2(B2)S3(B3)S4(B4)S5(B5)S6(B6)S7(B7)S8(B8)

Console.WriteLine(
    "Hello. You are at the start of the 8-th task program. I need to ask you, would you like to introduce data or to generate it?"
);
Console.WriteLine("Enter '1' to introduce data or '2' to generate it randomly.");
int choice = user_choose();

if (choice == 1)
{
    Console.WriteLine("You have chosen to introduce data.");
    Console.WriteLine("Please enter L[k-1] in hexadecimal format (e.g., 0x12345678):");
    //Ln_minus_one = Convert.ToUInt32(Console.ReadLine() ?? "0", 16);
    Ln_minus_one = user_choose_ln_minus_one();
    Console.WriteLine(
        "Please enter the result of the S-boxes in hexadecimal format (e.g., 0x9ABCDEF0):"
    );
    //sboxes_result = Convert.ToUInt32(Console.ReadLine() ?? "0", 16);
    sboxes_result = user_choose_sboxes_result();
    Console.WriteLine(
        $"You have provided L[k-1] = 0x{Ln_minus_one:X8} and S-boxes result = 0x{sboxes_result:X8}"
    );
    Console.WriteLine("Data collected");
}
else if (choice == 2)
{
    Console.WriteLine("You have chosen to generate data randomly.");
    Ln_minus_one = (uint)new Random().Next(0, int.MaxValue);
    sboxes_result = (uint)new Random().Next(0, int.MaxValue);
    Console.WriteLine(
        $"Randomly generated L[k-1] = 0x{Ln_minus_one:X8} and S-boxes result = 0x{sboxes_result:X8}"
    );
    Console.WriteLine("Data generated");
}

Console.WriteLine("=== Part of the algorithm started. ===");
var algorithm = new Algorithm(sboxes_result, Ln_minus_one); // Example values for sboxes_result and Ln_minus_one
Console.WriteLine($"\n[Result] R[k] = 0x{algorithm.getResult():X8}");
Console.WriteLine("=== Part of the algorithm finished. ===");

int user_choose()
{
    while (true)
    {
        string? input = Console.ReadLine();
        if (input is null)
        {
            Console.WriteLine("Input cannot be null. Please enter '1' or '2'.");
            continue;
        }
        if (input == "1" || input == "2")
        {
            return int.Parse(input);
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter '1' or '2'.");
        }
    }
}

uint user_choose_ln_minus_one()
{
    while (true)
    {
        string? input = Console.ReadLine();
        if (input is null)
        {
            Console.WriteLine("Input cannot be null. Please enter a hexadecimal number.");
            continue;
        }
        try
        {
            return Convert.ToUInt32(input, 16);
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid format. Please enter a valid hexadecimal number.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Number too large. Please enter a valid hexadecimal number.");
        }
    }
}

uint user_choose_sboxes_result()
{
    while (true)
    {
        string? input = Console.ReadLine();
        if (input is null)
        {
            Console.WriteLine("Input cannot be null. Please enter a hexadecimal number.");
            continue;
        }
        try
        {
            return Convert.ToUInt32(input, 16);
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid format. Please enter a valid hexadecimal number.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Number too large. Please enter a valid hexadecimal number.");
        }
    }
}
