namespace System.IO.Hashing;

/// <summary>
/// Represents a 128-bit hash value.
/// </summary>
/// <remarks>
/// Initializes a new instance of the Hash128 struct.
/// </remarks>
public readonly struct Hash128(ulong low, ulong high)
{
    /// <summary>
    /// Gets the low 64 bits of the hash.
    /// </summary>
    public ulong Low { get; } = low;

    /// <summary>
    /// Gets the high 64 bits of the hash.
    /// </summary>
    public ulong High { get; } = high;

    /// <summary>
    /// Returns a hexadecimal string representation of the hash.
    /// </summary>
    public override string ToString() => $"{High:X16}{Low:X16}";

    /// <summary>
    /// Converts the hash to a 16-byte array.
    /// </summary>
    public byte[] ToByteArray()
    {
        byte[] result = new byte[16];
        unsafe
        {
            fixed (byte* ptr = result)
            {
                *(ulong*)ptr = Low;
                *(ulong*)(ptr + 8) = High;
            }
        }
        return result;
    }

    /// <summary>
    /// Determines whether two Hash128 values are equal.
    /// </summary>
    public override bool Equals(object obj)
    {
        return obj is Hash128 other && Equals(other);
    }

    /// <summary>
    /// Determines whether two Hash128 values are equal.
    /// </summary>
    public bool Equals(Hash128 other)
    {
        return Low == other.Low && High == other.High;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    public override int GetHashCode()
    {
        unchecked
        {
            return (int)(Low ^ (Low >> 32) ^ High ^ (High >> 32));
        }
    }

    /// <summary>
    /// Determines whether two Hash128 values are equal.
    /// </summary>
    public static bool operator ==(Hash128 left, Hash128 right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Determines whether two Hash128 values are not equal.
    /// </summary>
    public static bool operator !=(Hash128 left, Hash128 right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Deconstructs the Hash128 into its low and high components.
    /// </summary>
    public void Deconstruct(out ulong low, out ulong high)
    {
        low = Low;
        high = High;
    }
}
