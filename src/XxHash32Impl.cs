using System.Runtime.CompilerServices;

namespace System.IO.Hashing;

internal static unsafe class XxHash32Impl
{
    private const uint Prime1 = 2654435761U;
    private const uint Prime2 = 2246822519U;
    private const uint Prime3 = 3266489917U;
    private const uint Prime4 = 668265263U;
    private const uint Prime5 = 374761393U;

    public static uint Hash(byte[] data, uint seed = 0)
    {
        if (data is null) return 0;
        fixed (byte* p = data)
        {
            return HashCore(p, data.Length, seed);
        }
    }

    public static uint Hash(string str, uint seed = 0)
    {
        if (str is null) return 0;
        fixed (char* p = str)
        {
            return HashCore((byte*)p, str.Length * sizeof(char), seed);
        }
    }

    private static uint HashCore(byte* input, int length, uint seed)
    {
        uint hash;
        byte* p = input;
        byte* end = p + length;

        if (length >= 16)
        {
            uint v1 = seed + Prime1 + Prime2;
            uint v2 = seed + Prime2;
            uint v3 = seed;
            uint v4 = seed - Prime1;

            byte* limit = end - 16;
            while (p <= limit)
            {
                v1 = Round(v1, *(uint*)p); p += 4;
                v2 = Round(v2, *(uint*)p); p += 4;
                v3 = Round(v3, *(uint*)p); p += 4;
                v4 = Round(v4, *(uint*)p); p += 4;
            }

            hash = Rotl(v1, 1) + Rotl(v2, 7) + Rotl(v3, 12) + Rotl(v4, 18);
        }
        else
        {
            hash = seed + Prime5;
        }

        hash += (uint)length;

        while (p + 4 <= end)
        {
            hash += *(uint*)p * Prime3;
            hash = Rotl(hash, 17) * Prime4;
            p += 4;
        }

        while (p < end)
        {
            hash += *p * Prime5;
            hash = Rotl(hash, 11) * Prime1;
            p++;
        }

        return FinalMix(hash);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Round(uint acc, uint input)
    {
        acc += input * Prime2;
        acc = Rotl(acc, 13);
        acc *= Prime1;
        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Rotl(uint v, int r) => (v << r) | (v >> (32 - r));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint FinalMix(uint hash)
    {
        hash ^= hash >> 15;
        hash *= Prime2;
        hash ^= hash >> 13;
        hash *= Prime3;
        hash ^= hash >> 16;
        return hash;
    }
}
