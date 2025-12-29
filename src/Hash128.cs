namespace System.IO.Hashing;

/// <summary>
/// Represents a 128-bit hash value.
/// </summary>
public struct Hash128
{
    /// <summary>
    /// Gets or sets the low 64 bits of the hash.
    /// </summary>
    public ulong Low;

    /// <summary>
    /// Gets or sets the high 64 bits of the hash.
    /// </summary>
    public ulong High;

    /// <summary>
    /// Initializes a new instance of the Hash128 struct.
    /// </summary>
    public Hash128(ulong low, ulong high)
    {
        Low = low;
        High = high;
    }

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
        if (obj is Hash128 other)
        {
            return Low == other.Low && High == other.High;
        }
        return false;
    }

    /// <summary>
    /// Returns the hash code for this instance.
    /// </summary>
    public override int GetHashCode()
    {
        return Low.GetHashCode() ^ High.GetHashCode();
    }

    /// <summary>
    /// Determines whether two Hash128 values are equal.
    /// </summary>
    public static bool operator ==(Hash128 left, Hash128 right)
    {
        return left.Low == right.Low && left.High == right.High;
    }

    /// <summary>
    /// Determines whether two Hash128 values are not equal.
    /// </summary>
    public static bool operator !=(Hash128 left, Hash128 right)
    {
        return !(left == right);
    }
}
