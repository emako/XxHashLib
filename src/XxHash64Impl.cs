using System.Runtime.CompilerServices;

namespace System.IO.Hashing;

internal static unsafe class XxHash64Impl
{
    private const ulong Prime1 = 11400714819323198393UL;
    private const ulong Prime2 = 14029467366897019727UL;
    private const ulong Prime3 = 1609587929392839161UL;
    private const ulong Prime4 = 9650029242287828579UL;
    private const ulong Prime5 = 2870177450012600261UL;

    public static ulong Hash(byte[] data, ulong seed = 0)
    {
        if (data is null) return 0;
        fixed (byte* p = data)
        {
            return HashCore(p, data.Length, seed);
        }
    }

    public static ulong Hash(string str, ulong seed = 0)
    {
        if (str is null) return 0;
        fixed (char* p = str)
        {
            return HashCore((byte*)p, str.Length * sizeof(char), seed);
        }
    }

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    public static ulong Hash(ReadOnlySpan<byte> data, ulong seed = 0)
    {
        fixed (byte* p = data)
        {
            return HashCore(p, data.Length, seed);
        }
    }

    public static ulong Hash(ReadOnlySpan<char> data, ulong seed = 0)
    {
        fixed (char* p = data)
        {
            return HashCore((byte*)p, data.Length * sizeof(char), seed);
        }
    }
#endif

    public static ulong Hash(Stream stream, ulong seed = 0, int bufferSize = 4096)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));

        byte[] buffer = new byte[bufferSize];
        ulong v1 = seed + Prime1 + Prime2;
        ulong v2 = seed + Prime2;
        ulong v3 = seed;
        ulong v4 = seed - Prime1;
        long totalLength = 0;
        bool hasProcessedBlock = false;
        int carryOver = 0;

        int bytesRead;
        while ((bytesRead = stream.Read(buffer, carryOver, buffer.Length - carryOver)) > 0)
        {
            int totalBytes = carryOver + bytesRead;
            totalLength += bytesRead;

            fixed (byte* bufferPtr = buffer)
            {
                byte* p = bufferPtr;
                byte* end = p + totalBytes;

                while (p + 32 <= end)
                {
                    hasProcessedBlock = true;
                    v1 = Round(v1, *(ulong*)p); p += 8;
                    v2 = Round(v2, *(ulong*)p); p += 8;
                    v3 = Round(v3, *(ulong*)p); p += 8;
                    v4 = Round(v4, *(ulong*)p); p += 8;
                }

                carryOver = (int)(end - p);
                if (carryOver > 0)
                {
                    for (int i = 0; i < carryOver; i++)
                    {
                        buffer[i] = p[i];
                    }
                }
            }
        }

        ulong hash;
        if (hasProcessedBlock)
        {
            hash = Rotl(v1, 1) + Rotl(v2, 7) + Rotl(v3, 12) + Rotl(v4, 18);
            hash = MergeRound(hash, v1);
            hash = MergeRound(hash, v2);
            hash = MergeRound(hash, v3);
            hash = MergeRound(hash, v4);
        }
        else
        {
            hash = seed + Prime5;
        }

        hash += (ulong)totalLength;

        if (carryOver > 0)
        {
            fixed (byte* tailPtr = buffer)
            {
                byte* tp = tailPtr;
                byte* tend = tp + carryOver;

                while (tp + 8 <= tend)
                {
                    ulong k1 = *(ulong*)tp;
                    k1 *= Prime2;
                    k1 = Rotl(k1, 31);
                    k1 *= Prime1;
                    hash ^= k1;
                    hash = Rotl(hash, 27) * Prime1 + Prime4;
                    tp += 8;
                }

                while (tp + 4 <= tend)
                {
                    hash ^= *(uint*)tp * Prime1;
                    hash = Rotl(hash, 23) * Prime2 + Prime3;
                    tp += 4;
                }

                while (tp < tend)
                {
                    hash ^= *tp * Prime5;
                    hash = Rotl(hash, 11) * Prime1;
                    tp++;
                }
            }
        }

        return FinalMix(hash);
    }

    private static ulong HashCore(byte* input, int length, ulong seed)
    {
        ulong hash;
        byte* p = input;
        byte* end = p + length;

        if (length >= 32)
        {
            ulong v1 = seed + Prime1 + Prime2;
            ulong v2 = seed + Prime2;
            ulong v3 = seed;
            ulong v4 = seed - Prime1;

            byte* limit = end - 32;
            while (p <= limit)
            {
                v1 = Round(v1, *(ulong*)p); p += 8;
                v2 = Round(v2, *(ulong*)p); p += 8;
                v3 = Round(v3, *(ulong*)p); p += 8;
                v4 = Round(v4, *(ulong*)p); p += 8;
            }

            hash = Rotl(v1, 1) + Rotl(v2, 7) + Rotl(v3, 12) + Rotl(v4, 18);

            hash = MergeRound(hash, v1);
            hash = MergeRound(hash, v2);
            hash = MergeRound(hash, v3);
            hash = MergeRound(hash, v4);
        }
        else
        {
            hash = seed + Prime5;
        }

        hash += (ulong)length;

        while (p + 8 <= end)
        {
            ulong k1 = *(ulong*)p;
            k1 *= Prime2;
            k1 = Rotl(k1, 31);
            k1 *= Prime1;
            hash ^= k1;
            hash = Rotl(hash, 27) * Prime1 + Prime4;
            p += 8;
        }

        while (p + 4 <= end)
        {
            hash ^= *(uint*)p * Prime1;
            hash = Rotl(hash, 23) * Prime2 + Prime3;
            p += 4;
        }

        while (p < end)
        {
            hash ^= *p * Prime5;
            hash = Rotl(hash, 11) * Prime1;
            p++;
        }

        return FinalMix(hash);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Round(ulong acc, ulong input)
    {
        acc += input * Prime2;
        acc = Rotl(acc, 31);
        acc *= Prime1;
        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong MergeRound(ulong acc, ulong val)
    {
        val = Round(0, val);
        acc ^= val;
        acc = acc * Prime1 + Prime4;
        return acc;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Rotl(ulong v, int r) => (v << r) | (v >> (64 - r));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong FinalMix(ulong hash)
    {
        hash ^= hash >> 33;
        hash *= Prime2;
        hash ^= hash >> 29;
        hash *= Prime3;
        hash ^= hash >> 32;
        return hash;
    }
}
