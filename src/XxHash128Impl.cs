using System.Runtime.CompilerServices;

namespace System.IO.Hashing;

internal static unsafe class XxHash128Impl
{
    private const ulong Prime64_1 = 0x9E3779B185EBCA87UL;
    private const ulong Prime64_2 = 0xC2B2AE3D27D4EB4FUL;
    private const ulong Prime64_3 = 0x165667B19E3779F9UL;
    private const ulong Prime64_4 = 0x85EBCA77C2B2AE63UL;
    private const ulong Prime64_5 = 0x27D4EB2F165667C5UL;

    private static readonly byte[] Secret = new byte[]
    {
        0xb8, 0xfe, 0x6c, 0x39, 0x23, 0xa4, 0x4b, 0xbe, 0x7c, 0x01, 0x81, 0x2c, 0xf7, 0x21, 0xad, 0x1c,
        0xde, 0xd4, 0x6d, 0xe9, 0x83, 0x90, 0x97, 0xdb, 0x72, 0x40, 0xa4, 0xa4, 0xb7, 0xb3, 0x67, 0x1f,
        0xcb, 0x79, 0xe6, 0x4e, 0xcc, 0xc0, 0xe5, 0x78, 0x82, 0x5a, 0xd0, 0x7d, 0xcc, 0xff, 0x72, 0x21,
        0xb8, 0x08, 0x46, 0x74, 0xf7, 0x43, 0x24, 0x8e, 0xe0, 0x35, 0x90, 0xe6, 0x81, 0x3a, 0x26, 0x4c,
        0x3c, 0x28, 0x52, 0xbb, 0x91, 0xc3, 0x00, 0xcb, 0x88, 0xd0, 0x65, 0x8b, 0x1b, 0x53, 0x2e, 0xa3,
        0x71, 0x64, 0x48, 0x97, 0xa2, 0x0d, 0xf9, 0x4e, 0x38, 0x19, 0xef, 0x46, 0xa9, 0xde, 0xac, 0xd8,
        0xa8, 0xfa, 0x76, 0x3f, 0xe3, 0x9c, 0x34, 0x3f, 0xf9, 0xdc, 0xbb, 0xc7, 0xc7, 0x0b, 0x4f, 0x1d,
        0x8a, 0x51, 0xe0, 0x4b, 0xcd, 0xb4, 0x59, 0x31, 0xc8, 0x9f, 0x7e, 0xc9, 0xd9, 0x78, 0x73, 0x64,
        0xea, 0xc5, 0xac, 0x83, 0x34, 0xd3, 0xeb, 0xc3, 0xc5, 0x81, 0xa0, 0xff, 0xfa, 0x13, 0x63, 0xeb,
        0x17, 0x0d, 0xdd, 0x51, 0xb7, 0xf0, 0xda, 0x49, 0xd3, 0x16, 0x55, 0x26, 0x29, 0xd4, 0x68, 0x9e,
        0x2b, 0x16, 0xbe, 0x58, 0x7d, 0x47, 0xa1, 0xfc, 0x8f, 0xf8, 0xb8, 0xd1, 0x7a, 0xd0, 0x31, 0xce,
        0x45, 0xcb, 0x3a, 0x8f, 0x95, 0x16, 0x04, 0x28, 0xaf, 0xd7, 0xfb, 0xca, 0xbb, 0x4b, 0x40, 0x7e,
    };

    public static Hash128 Hash(byte[] data, ulong seed = 0)
    {
        if (data is null) return new Hash128(0, 0);
        fixed (byte* p = data)
        {
            return HashCore(p, data.Length, seed);
        }
    }

    public static Hash128 Hash(string str, ulong seed = 0)
    {
        if (str is null) return new Hash128(0, 0);
        fixed (char* p = str)
        {
            return HashCore((byte*)p, str.Length * sizeof(char), seed);
        }
    }

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    public static Hash128 Hash(ReadOnlySpan<byte> data, ulong seed = 0)
    {
        fixed (byte* p = data)
        {
            return HashCore(p, data.Length, seed);
        }
    }

    public static Hash128 Hash(ReadOnlySpan<char> data, ulong seed = 0)
    {
        fixed (char* p = data)
        {
            return HashCore((byte*)p, data.Length * sizeof(char), seed);
        }
    }
#endif

    public static Hash128 Hash(Stream stream, ulong seed = 0, int bufferSize = 4096)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));

        var allData = new System.Collections.Generic.List<byte>();
        byte[] buffer = new byte[bufferSize];
        int bytesRead;

        while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            for (int i = 0; i < bytesRead; i++)
            {
                allData.Add(buffer[i]);
            }
        }

        byte[] data = allData.ToArray();
        fixed (byte* p = data)
        {
            return HashCore(p, data.Length, seed);
        }
    }

    private static Hash128 HashCore(byte* input, int length, ulong seed)
    {
        if (length <= 16)
            return HashLen0To16(input, length, seed);

        if (length <= 128)
            return HashLen17To128(input, length, seed);

        if (length <= 240)
            return HashLen129To240(input, length, seed);

        return HashLong(input, length, seed);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Hash128 HashLen0To16(byte* input, int length, ulong seed)
    {
        if (length > 8)
            return HashLen9To16(input, length, seed);

        if (length >= 4)
            return HashLen4To8(input, length, seed);

        if (length > 0)
            return HashLen1To3(input, length, seed);

        ulong low = XXH64_avalanche(seed ^ (ReadU64(Secret, 56) ^ ReadU64(Secret, 64)));
        ulong high = XXH64_avalanche(seed ^ (ReadU64(Secret, 72) ^ ReadU64(Secret, 80)));
        return new Hash128(low, high);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Hash128 HashLen1To3(byte* input, int length, ulong seed)
    {
        byte c1 = input[0];
        byte c2 = input[length >> 1];
        byte c3 = input[length - 1];
        uint combined = ((uint)c1 << 16) | ((uint)c2 << 24) | ((uint)c3 << 0) | ((uint)length << 8);

        ulong bitflip1 = (ReadU32(Secret, 0) ^ ReadU32(Secret, 4)) + seed;
        ulong bitflip2 = (ReadU32(Secret, 8) ^ ReadU32(Secret, 12)) - seed;
        ulong keyed1 = (ulong)combined ^ bitflip1;
        ulong keyed2 = (ulong)combined ^ bitflip2;
        return new Hash128(XXH64_avalanche(keyed1), XXH64_avalanche(keyed2));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Hash128 HashLen4To8(byte* input, int length, ulong seed)
    {
        seed ^= (ulong)Swap32((uint)seed) << 32;
        uint input1 = ReadU32(input, 0);
        uint input2 = ReadU32(input, length - 4);

        ulong bitflip1 = (ReadU64(Secret, 8) ^ ReadU64(Secret, 16)) - seed;
        ulong bitflip2 = (ReadU64(Secret, 24) ^ ReadU64(Secret, 32)) + seed;
        ulong input64 = input2 + (((ulong)input1) << 32);

        ulong keyed1 = input64 ^ bitflip1;
        ulong keyed2 = input64 ^ bitflip2;

        return new Hash128(
            XXH3_rrmxmx(keyed1, (ulong)length),
            XXH3_rrmxmx(keyed2, (ulong)length)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Hash128 HashLen9To16(byte* input, int length, ulong seed)
    {
        ulong bitflip1 = (ReadU64(Secret, 24) ^ ReadU64(Secret, 32)) + seed;
        ulong bitflip2 = (ReadU64(Secret, 40) ^ ReadU64(Secret, 48)) - seed;
        ulong input_lo = ReadU64(input, 0) ^ bitflip1;
        ulong input_hi = ReadU64(input, length - 8) ^ bitflip2;

        ulong acc_low = (ulong)length + Swap64(input_lo) + input_hi + XXH3_mul128_fold64(input_lo, input_hi);
        ulong acc_high = (ulong)length + Swap64(input_hi) + input_lo + XXH3_mul128_fold64(input_hi, input_lo);

        return new Hash128(XXH3_avalanche(acc_low), XXH3_avalanche(acc_high));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Hash128 HashLen17To128(byte* input, int length, ulong seed)
    {
        ulong acc_low = (ulong)length * Prime64_1;
        ulong acc_high = 0;

        if (length > 32)
        {
            if (length > 64)
            {
                if (length > 96)
                {
                    acc_low += XXH3_mix16B(input + 48, Secret, 96, seed);
                    acc_high += XXH3_mix16B(input + length - 64, Secret, 112, seed);
                }
                acc_low += XXH3_mix16B(input + 32, Secret, 64, seed);
                acc_high += XXH3_mix16B(input + length - 48, Secret, 80, seed);
            }
            acc_low += XXH3_mix16B(input + 16, Secret, 32, seed);
            acc_high += XXH3_mix16B(input + length - 32, Secret, 48, seed);
        }
        acc_low += XXH3_mix16B(input + 0, Secret, 0, seed);
        acc_high += XXH3_mix16B(input + length - 16, Secret, 16, seed);

        return new Hash128(XXH3_avalanche(acc_low), XXH3_avalanche(acc_high));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Hash128 HashLen129To240(byte* input, int length, ulong seed)
    {
        ulong acc_low = (ulong)length * Prime64_1;
        ulong acc_high = 0;
        int nbRounds = length / 16;

        for (int i = 0; i < 8; i++)
        {
            acc_low += XXH3_mix16B(input + (16 * i), Secret, 16 * i, seed);
        }
        acc_low = XXH3_avalanche(acc_low);

        for (int i = 8; i < nbRounds; i++)
        {
            acc_high += XXH3_mix16B(input + (16 * i), Secret, 16 * (i - 8) + 3, seed);
        }

        acc_high += XXH3_mix16B(input + length - 16, Secret, 119, seed);
        return new Hash128(XXH3_avalanche(acc_low), XXH3_avalanche(acc_high));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Hash128 HashLong(byte* input, int length, ulong seed)
    {
        ulong acc_low = Prime64_1 * (ulong)length;
        ulong acc_high = Prime64_2 * (ulong)length;
        int nbStripes = (length - 1) / 64;

        for (int n = 0; n < nbStripes; n++)
        {
            for (int i = 0; i < 16; i += 2)
            {
                ulong data_val = ReadU64(input, n * 64 + i * 4);
                ulong secret_val = ReadU64(Secret, i * 4);
                acc_low += XXH3_mix16B_single(data_val, secret_val ^ (seed + (ulong)(n * 64)));
                acc_high += XXH3_mix16B_single(data_val, secret_val ^ (seed + (ulong)(n * 64) + 1));
            }
        }

        int remaining = length - nbStripes * 64;
        if (remaining > 0)
        {
            byte* lastStripe = input + length - 64;
            for (int i = 0; i < 16; i += 2)
            {
                ulong data_val = ReadU64(lastStripe, i * 4);
                ulong secret_val = ReadU64(Secret, i * 4);
                acc_low += XXH3_mix16B_single(data_val, secret_val ^ seed);
                acc_high += XXH3_mix16B_single(data_val, secret_val ^ (seed + 1));
            }
        }

        return new Hash128(XXH3_avalanche(acc_low), XXH3_avalanche(acc_high));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH3_mix16B(byte* input, byte[] secret, int secretOffset, ulong seed)
    {
        ulong input_lo = ReadU64(input, 0);
        ulong input_hi = ReadU64(input, 8);
        return XXH3_mul128_fold64(
            input_lo ^ (ReadU64(secret, secretOffset) + seed),
            input_hi ^ (ReadU64(secret, secretOffset + 8) - seed)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH3_mix16B_single(ulong data, ulong secret)
    {
        ulong lo = data & 0xFFFFFFFF;
        ulong hi = data >> 32;
        return lo * (secret & 0xFFFFFFFF) + hi * (secret >> 32);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH3_mul128_fold64(ulong lhs, ulong rhs)
    {
        ulong lo = (lhs & 0xFFFFFFFF) * (rhs & 0xFFFFFFFF);
        ulong hi = (lhs >> 32) * (rhs >> 32);
        ulong cross = (lhs >> 32) * (rhs & 0xFFFFFFFF) + (lhs & 0xFFFFFFFF) * (rhs >> 32);
        return lo + (cross << 32) + hi;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH3_avalanche(ulong hash)
    {
        hash ^= hash >> 37;
        hash *= 0x165667919E3779F9UL;
        hash ^= hash >> 32;
        return hash;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH3_rrmxmx(ulong h64, ulong length)
    {
        h64 ^= Rotl64(h64, 49) ^ Rotl64(h64, 24);
        h64 *= 0x9FB21C651E98DF25UL;
        h64 ^= (h64 >> 35) + length;
        h64 *= 0x9FB21C651E98DF25UL;
        return h64 ^ (h64 >> 28);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong XXH64_avalanche(ulong hash)
    {
        hash ^= hash >> 33;
        hash *= Prime64_2;
        hash ^= hash >> 29;
        hash *= Prime64_3;
        hash ^= hash >> 32;
        return hash;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong ReadU64(byte* ptr, int offset)
    {
        return *(ulong*)(ptr + offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong ReadU64(byte[] array, int offset)
    {
        return ((ulong)array[offset + 0] << 0) |
               ((ulong)array[offset + 1] << 8) |
               ((ulong)array[offset + 2] << 16) |
               ((ulong)array[offset + 3] << 24) |
               ((ulong)array[offset + 4] << 32) |
               ((ulong)array[offset + 5] << 40) |
               ((ulong)array[offset + 6] << 48) |
               ((ulong)array[offset + 7] << 56);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ReadU32(byte* ptr, int offset)
    {
        return *(uint*)(ptr + offset);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint ReadU32(byte[] array, int offset)
    {
        return ((uint)array[offset + 0] << 0) |
               ((uint)array[offset + 1] << 8) |
               ((uint)array[offset + 2] << 16) |
               ((uint)array[offset + 3] << 24);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Rotl64(ulong v, int r) => (v << r) | (v >> (64 - r));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ulong Swap64(ulong v)
    {
        return ((v & 0x00000000000000FFUL) << 56) |
               ((v & 0x000000000000FF00UL) << 40) |
               ((v & 0x0000000000FF0000UL) << 24) |
               ((v & 0x00000000FF000000UL) << 8) |
               ((v & 0x000000FF00000000UL) >> 8) |
               ((v & 0x0000FF0000000000UL) >> 24) |
               ((v & 0x00FF000000000000UL) >> 40) |
               ((v & 0xFF00000000000000UL) >> 56);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static uint Swap32(uint v)
    {
        return ((v & 0x000000FFU) << 24) |
               ((v & 0x0000FF00U) << 8) |
               ((v & 0x00FF0000U) >> 8) |
               ((v & 0xFF000000U) >> 24);
    }
}
