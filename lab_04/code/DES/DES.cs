namespace DES;

public class DES
{
    private ulong message; // 64 bits
    private ulong key; // 64 bits

    private ulong[] subkeys = new ulong[16]; // each 48 bits

    public DES(ulong message, ulong key)
    {
        this.message = message;
        this.key = key;
    }

    private void keyGeneration()
    {
        // Apply PC-1 to obtain a 56-bit key.
        ulong K_plus = PC_one_permutation(this.key);

        // Split into two 28-bit halves.
        uint C0 = (uint)(K_plus >> 28);
        uint D0 = (uint)(K_plus & 0x0FFFFFFF);

        // Generate Cn and Dn for all 16 rounds.
        uint[] Cn = new uint[17];
        uint[] Dn = new uint[17];

        Cn[0] = C0;
        Dn[0] = D0;

        for (int i = 1; i <= 16; i++)
        {
            int shift = (i == 1 || i == 2 || i == 9 || i == 16) ? 1 : 2;

            // Circular left shifts within 28 bits.
            Cn[i] = ((Cn[i - 1] << shift) | (Cn[i - 1] >> (28 - shift))) & 0x0FFFFFFF;

            Dn[i] = ((Dn[i - 1] << shift) | (Dn[i - 1] >> (28 - shift))) & 0x0FFFFFFF;
        }

        // Concatenate Cn and Dn into 56-bit values.
        ulong[] CnDn = new ulong[17];

        for (int i = 0; i <= 16; i++)
        {
            CnDn[i] = ((ulong)Cn[i] << 28) | Dn[i];
        }

        // Generate 16 subkeys using PC-2.
        for (int i = 1; i <= 16; i++)
        {
            this.subkeys[i - 1] = PC_two_permutation(CnDn[i]);
        }
    }

    private ulong PC_two_permutation(ulong CnDn)
    {
        // PC-2: 56 bits -> 48 bits.

        // csharpier-ignore-start
        int[] pc2 =
        {
            14, 17, 11, 24,  1,  5,
             3, 28, 15,  6, 21, 10,
            23, 19, 12,  4, 26,  8,
            16,  7, 27, 20, 13,  2,
            41, 52, 31, 37, 47, 55,
            30, 40, 51, 45, 33, 48,
            44, 49, 39, 56, 34, 53,
            46, 42, 50, 36, 29, 32
        };
        // csharpier-ignore-end

        ulong permutedKey = 0;

        for (int i = 0; i < pc2.Length; i++)
        {
            permutedKey <<= 1;
            permutedKey |= (CnDn >> (56 - pc2[i])) & 1UL;
        }

        return permutedKey;
    }

    private ulong PC_one_permutation(ulong key)
    {
        // PC-1: 64 bits -> 56 bits.

        // csharpier-ignore-start
        int[] pc1 =
        {
            57, 49, 41, 33, 25, 17,  9,
             1, 58, 50, 42, 34, 26, 18,
            10,  2, 59, 51, 43, 35, 27,
            19, 11,  3, 60, 52, 44, 36,
            63, 55, 47, 39, 31, 23, 15,
             7, 62, 54, 46, 38, 30, 22,
            14,  6, 61, 53, 45, 37, 29,
            21, 13,  5, 28, 20, 12,  4
        };
        // csharpier-ignore-end

        ulong permutedKey = 0;

        for (int i = 0; i < pc1.Length; i++)
        {
            permutedKey <<= 1;
            permutedKey |= (key >> (64 - pc1[i])) & 1UL;
        }

        return permutedKey;
    }
}
