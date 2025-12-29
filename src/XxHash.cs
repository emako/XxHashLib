namespace System.IO.Hashing;

/// <summary>
/// xxHash - Extremely fast non-cryptographic hash algorithm
/// </summary>
public static class XxHash
{
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
}
