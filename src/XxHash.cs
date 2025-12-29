namespace System.IO.Hashing;

/// <summary>
/// xxHash - Extremely fast non-cryptographic hash algorithm
/// </summary>
public static class XxHash
{
    #region XxHash64

    /// <summary>
    /// Computes the 64-bit xxHash of the given string (interpreting chars as bytes).
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash64(string str)
        => XxHash64Impl.Hash(str);

    /// <summary>
    /// Computes the 64-bit xxHash of the given byte array.
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash64(byte[] data)
        => XxHash64Impl.Hash(data);

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Computes the 64-bit xxHash of the given byte span.
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash64(ReadOnlySpan<byte> data)
        => XxHash64Impl.Hash(data);

    /// <summary>
    /// Computes the 64-bit xxHash of the given char span (interpreting chars as bytes).
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash64(ReadOnlySpan<char> data)
        => XxHash64Impl.Hash(data);
#endif

    /// <summary>
    /// Computes the 64-bit xxHash of the given stream.
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash64(Stream stream, int bufferSize = 4096)
        => XxHash64Impl.Hash(stream, 0, bufferSize);

    /// <summary>
    /// Computes the 64-bit xxHash of the given string (interpreting chars as bytes).
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash64s(string str)
        => (long)XxHash64Impl.Hash(str);

    /// <summary>
    /// Computes the 64-bit xxHash of the given byte array.
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash64s(byte[] data)
        => (long)XxHash64Impl.Hash(data);

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Computes the 64-bit xxHash of the given byte span.
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash64s(ReadOnlySpan<byte> data)
        => (long)XxHash64Impl.Hash(data);

    /// <summary>
    /// Computes the 64-bit xxHash of the given char span (interpreting chars as bytes).
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash64s(ReadOnlySpan<char> data)
        => (long)XxHash64Impl.Hash(data);
#endif

    /// <summary>
    /// Computes the 64-bit xxHash of the given stream.
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash64s(Stream stream, int bufferSize = 4096)
        => (long)XxHash64Impl.Hash(stream, 0, bufferSize);

    #endregion XxHash64

    #region XxHash32

    /// <summary>
    /// Computes the 32-bit xxHash of the given string (interpreting chars as bytes).
    /// Returns an unsigned 32-bit integer.
    /// </summary>
    public static uint XxHash32(string str)
        => XxHash32Impl.Hash(str);

    /// <summary>
    /// Computes the 32-bit xxHash of the given byte array.
    /// Returns an unsigned 32-bit integer.
    /// </summary>
    public static uint XxHash32(byte[] data)
        => XxHash32Impl.Hash(data);

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Computes the 32-bit xxHash of the given byte span.
    /// Returns an unsigned 32-bit integer.
    /// </summary>
    public static uint XxHash32(ReadOnlySpan<byte> data)
        => XxHash32Impl.Hash(data);

    /// <summary>
    /// Computes the 32-bit xxHash of the given char span (interpreting chars as bytes).
    /// Returns an unsigned 32-bit integer.
    /// </summary>
    public static uint XxHash32(ReadOnlySpan<char> data)
        => XxHash32Impl.Hash(data);
#endif

    /// <summary>
    /// Computes the 32-bit xxHash of the given stream.
    /// Returns an unsigned 32-bit integer.
    /// </summary>
    public static uint XxHash32(Stream stream, int bufferSize = 4096)
        => XxHash32Impl.Hash(stream, 0, bufferSize);

    /// <summary>
    /// Computes the 32-bit xxHash of the given string (interpreting chars as bytes).
    /// Returns a signed 32-bit integer.
    /// </summary>
    public static int XxHash32s(string str)
        => (int)XxHash32Impl.Hash(str);

    /// <summary>
    /// Computes the 32-bit xxHash of the given byte array.
    /// Returns a signed 32-bit integer.
    /// </summary>
    public static int XxHash32s(byte[] data)
        => (int)XxHash32Impl.Hash(data);

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Computes the 32-bit xxHash of the given byte span.
    /// Returns a signed 32-bit integer.
    /// </summary>
    public static int XxHash32s(ReadOnlySpan<byte> data)
        => (int)XxHash32Impl.Hash(data);

    /// <summary>
    /// Computes the 32-bit xxHash of the given char span (interpreting chars as bytes).
    /// Returns a signed 32-bit integer.
    /// </summary>
    public static int XxHash32s(ReadOnlySpan<char> data)
        => (int)XxHash32Impl.Hash(data);
#endif

    /// <summary>
    /// Computes the 32-bit xxHash of the given stream.
    /// Returns a signed 32-bit integer.
    /// </summary>
    public static int XxHash32s(Stream stream, int bufferSize = 4096)
        => (int)XxHash32Impl.Hash(stream, 0, bufferSize);

    #endregion XxHash32

    #region XxHash3

    /// <summary>
    /// Computes the 64-bit xxHash3 of the given string (interpreting chars as bytes).
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash3(string str)
        => XxHash3Impl.Hash(str);

    /// <summary>
    /// Computes the 64-bit xxHash3 of the given byte array.
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash3(byte[] data)
        => XxHash3Impl.Hash(data);

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Computes the 64-bit xxHash3 of the given byte span.
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash3(ReadOnlySpan<byte> data)
        => XxHash3Impl.Hash(data);

    /// <summary>
    /// Computes the 64-bit xxHash3 of the given char span (interpreting chars as bytes).
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash3(ReadOnlySpan<char> data)
        => XxHash3Impl.Hash(data);
#endif

    /// <summary>
    /// Computes the 64-bit xxHash3 of the given stream.
    /// Returns an unsigned 64-bit integer.
    /// </summary>
    public static ulong XxHash3(Stream stream, int bufferSize = 4096)
        => XxHash3Impl.Hash(stream, 0, bufferSize);

    /// <summary>
    /// Computes the 64-bit xxHash3 of the given string (interpreting chars as bytes).
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash3s(string str)
        => (long)XxHash3Impl.Hash(str);

    /// <summary>
    /// Computes the 64-bit xxHash3 of the given byte array.
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash3s(byte[] data)
        => (long)XxHash3Impl.Hash(data);

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Computes the 64-bit xxHash3 of the given byte span.
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash3s(ReadOnlySpan<byte> data)
        => (long)XxHash3Impl.Hash(data);

    /// <summary>
    /// Computes the 64-bit xxHash3 of the given char span (interpreting chars as bytes).
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash3s(ReadOnlySpan<char> data)
        => (long)XxHash3Impl.Hash(data);
#endif

    /// <summary>
    /// Computes the 64-bit xxHash3 of the given stream.
    /// Returns a signed 64-bit integer.
    /// </summary>
    public static long XxHash3s(Stream stream, int bufferSize = 4096)
        => (long)XxHash3Impl.Hash(stream, 0, bufferSize);

    #endregion XxHash3

    #region XxHash128

    /// <summary>
    /// Computes the 128-bit xxHash of the given string (interpreting chars as bytes).
    /// Returns a Hash128 structure.
    /// </summary>
    public static Hash128 XxHash128(string str)
        => XxHash128Impl.Hash(str);

    /// <summary>
    /// Computes the 128-bit xxHash of the given byte array.
    /// Returns a Hash128 structure.
    /// </summary>
    public static Hash128 XxHash128(byte[] data)
        => XxHash128Impl.Hash(data);

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    /// <summary>
    /// Computes the 128-bit xxHash of the given byte span.
    /// Returns a Hash128 structure.
    /// </summary>
    public static Hash128 XxHash128(ReadOnlySpan<byte> data)
        => XxHash128Impl.Hash(data);

    /// <summary>
    /// Computes the 128-bit xxHash of the given char span (interpreting chars as bytes).
    /// Returns a Hash128 structure.
    /// </summary>
    public static Hash128 XxHash128(ReadOnlySpan<char> data)
        => XxHash128Impl.Hash(data);
#endif

    /// <summary>
    /// Computes the 128-bit xxHash of the given stream.
    /// Returns a Hash128 structure.
    /// </summary>
    public static Hash128 XxHash128(Stream stream, int bufferSize = 4096)
        => XxHash128Impl.Hash(stream, 0, bufferSize);

    #endregion XxHash128
}
