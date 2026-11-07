namespace task_08;

// .8. Să se calculeze Ri pentru runda k a algoritmului DES,
// dacă se cunoaște Lk-1 și rezultatul aplicării cutiilor-S.
public class Algorithm
{
    private uint sboxes_result; // 32 bit
    private uint Ln_minus_one; // 32 bit

    private uint Rk; // 32 bit

    public Algorithm(uint sboxes_result, uint Ln_minus_one)
    {
        this.sboxes_result = sboxes_result;
        this.Ln_minus_one = Ln_minus_one;

        Console.WriteLine($"You have provided the following inputs:");
        Console.WriteLine($"L[k-1] = 0x{this.Ln_minus_one:X8}");
        Console.WriteLine($"S-boxes result = 0x{this.sboxes_result:X8}");

        Console.WriteLine("\nThe same in binary format:");
        Console.WriteLine($"L[k-1] = {Convert.ToString(this.Ln_minus_one, 2).PadLeft(32, '0')}");
        Console.WriteLine(
            $"S-boxes result = {Convert.ToString(this.sboxes_result, 2).PadLeft(32, '0')}"
        );

        Console.WriteLine("\nStep 1: Apply permutation P to the S-boxes result to obtain f.");
        uint permutated = P(this.sboxes_result);
        Console.WriteLine(
            $"Result of permutation P: f = 0x{permutated:X8} ({Convert.ToString(permutated, 2).PadLeft(32, '0')})"
        );
        Console.WriteLine("\nStep 2: Calculate R[k] by performing XOR between L[k-1] and f.");
        this.Rk = this.Ln_minus_one ^ permutated;
        Console.WriteLine(
            $"R[k] = L[k-1] XOR f = 0x{this.Ln_minus_one:X8} XOR 0x{permutated:X8} = 0x{this.Rk:X8}"
        );
        Console.WriteLine("The XORing in binary:");
        Console.WriteLine(
            $"{Convert.ToString(this.Ln_minus_one, 2).PadLeft(32, '0')} XOR {Convert.ToString(permutated, 2).PadLeft(32, '0')} = {Convert.ToString(this.Rk, 2).PadLeft(32, '0')}"
        );
    }

    public uint getResult()
    {
        return this.Rk;
    }

    private uint P(uint input)
    {
        // csharpier-ignore-start
        byte[] P_table = new byte[32] {
          16, 7, 20, 21,
          29, 12, 28, 17,
          1, 15, 23, 26,
          5, 18, 31, 10,
          2, 8, 24, 14,
          32, 27, 3, 9,
          19, 13, 30, 6,
          22, 11, 4, 25
        };
        // csharpier-ignore-end
        Console.WriteLine($"Permutation P table:");
        Console.WriteLine(string.Join(", ", P_table));
        Console.WriteLine(
            $"Input to permutation P: 0x{input:X8} ({Convert.ToString(input, 2).PadLeft(32, '0')})"
        );

        uint result = 0;
        for (byte i = 0; i < 32; i++)
        {
            // take the i-th bit and put it on the last place, then, shift left

            byte i_th_bit_index = (byte)(P_table[i] - 1);
            byte i_th_bit_value = (byte)((input >> (32 - i_th_bit_index - 1)) & 1);

            result = (result << 1) | i_th_bit_value;
        }

        return result;
    }
}
