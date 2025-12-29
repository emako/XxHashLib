using NUnit.Framework;
using System.IO.Hashing;
using System.Text;

namespace System.IO.Hashing.Tests;

[TestFixture]
public class XxHashTests
{
    private const string TestString = "The quick brown fox jumps over the lazy dog";
    private static readonly byte[] TestBytes = Encoding.UTF8.GetBytes(TestString);

    #region XxHash64 Tests

    [Test]
    public void XxHash64_String_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash64(TestString);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_String_Null_ReturnsZero()
    {
        var hash = XxHash.XxHash64((string)null);
        Assert.That(hash, Is.Zero);
    }

    [Test]
    public void XxHash64_ByteArray_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash64(TestBytes);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_ByteArray_Null_ReturnsZero()
    {
        var hash = XxHash.XxHash64((byte[])null);
        Assert.That(hash, Is.Zero);
    }

    [Test]
    public void XxHash64_EmptyByteArray_ReturnsNonZero()
    {
        var hash = XxHash.XxHash64(Array.Empty<byte>());
        Assert.That(hash, Is.Not.Zero);
    }

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    [Test]
    public void XxHash64_ByteSpan_ReturnsExpectedHash()
    {
        ReadOnlySpan<byte> span = TestBytes;
        var hash = XxHash.XxHash64(span);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_CharSpan_ReturnsExpectedHash()
    {
        ReadOnlySpan<char> span = TestString.AsSpan();
        var hash = XxHash.XxHash64(span);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_EmptyByteSpan_ReturnsNonZero()
    {
        ReadOnlySpan<byte> span = Array.Empty<byte>();
        var hash = XxHash.XxHash64(span);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_ByteSpan_MatchesByteArray()
    {
        ReadOnlySpan<byte> span = TestBytes;
        var spanHash = XxHash.XxHash64(span);
        var arrayHash = XxHash.XxHash64(TestBytes);
        Assert.That(spanHash, Is.EqualTo(arrayHash));
    }

    [Test]
    public void XxHash64_CharSpan_MatchesString()
    {
        ReadOnlySpan<char> span = TestString.AsSpan();
        var spanHash = XxHash.XxHash64(span);
        var stringHash = XxHash.XxHash64(TestString);
        Assert.That(spanHash, Is.EqualTo(stringHash));
    }
#endif

    [Test]
    public void XxHash64_Stream_ReturnsExpectedHash()
    {
        using var stream = new MemoryStream(TestBytes);
        var hash = XxHash.XxHash64(stream);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_Stream_MatchesByteArray()
    {
        using var stream = new MemoryStream(TestBytes);
        var streamHash = XxHash.XxHash64(stream);
        var arrayHash = XxHash.XxHash64(TestBytes);
        Assert.That(streamHash, Is.EqualTo(arrayHash));
    }

    [Test]
    public void XxHash64_Stream_EmptyStream_ReturnsNonZero()
    {
        using var stream = new MemoryStream();
        var hash = XxHash.XxHash64(stream);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_Stream_Null_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => XxHash.XxHash64((Stream)null));
    }

    [Test]
    public void XxHash64_Stream_CustomBufferSize_ReturnsExpectedHash()
    {
        using var stream = new MemoryStream(TestBytes);
        var hash = XxHash.XxHash64(stream, 16);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_Stream_LargeData_ReturnsExpectedHash()
    {
        var largeData = new byte[10000];
        new Random(42).NextBytes(largeData);
        using var stream = new MemoryStream(largeData);
        var hash = XxHash.XxHash64(stream);
        Assert.That(hash, Is.Not.Zero);
    }

    #endregion XxHash64 Tests

    #region XxHash64s Tests (Signed)

    [Test]
    public void XxHash64s_String_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash64s(TestString);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64s_ByteArray_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash64s(TestBytes);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64s_MatchesUnsignedCast()
    {
        var unsignedHash = XxHash.XxHash64(TestString);
        var signedHash = XxHash.XxHash64s(TestString);
        Assert.That(signedHash, Is.EqualTo((long)unsignedHash));
    }

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    [Test]
    public void XxHash64s_ByteSpan_ReturnsExpectedHash()
    {
        ReadOnlySpan<byte> span = TestBytes;
        var hash = XxHash.XxHash64s(span);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64s_CharSpan_ReturnsExpectedHash()
    {
        ReadOnlySpan<char> span = TestString.AsSpan();
        var hash = XxHash.XxHash64s(span);
        Assert.That(hash, Is.Not.Zero);
    }
#endif

    [Test]
    public void XxHash64s_Stream_ReturnsExpectedHash()
    {
        using var stream = new MemoryStream(TestBytes);
        var hash = XxHash.XxHash64s(stream);
        Assert.That(hash, Is.Not.Zero);
    }

    #endregion XxHash64s Tests (Signed)

    #region XxHash32 Tests

    [Test]
    public void XxHash32_String_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash32(TestString);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_String_Null_ReturnsZero()
    {
        var hash = XxHash.XxHash32((string)null);
        Assert.That(hash, Is.Zero);
    }

    [Test]
    public void XxHash32_ByteArray_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash32(TestBytes);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_ByteArray_Null_ReturnsZero()
    {
        var hash = XxHash.XxHash32((byte[])null);
        Assert.That(hash, Is.Zero);
    }

    [Test]
    public void XxHash32_EmptyByteArray_ReturnsNonZero()
    {
        var hash = XxHash.XxHash32(Array.Empty<byte>());
        Assert.That(hash, Is.Not.Zero);
    }

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    [Test]
    public void XxHash32_ByteSpan_ReturnsExpectedHash()
    {
        ReadOnlySpan<byte> span = TestBytes;
        var hash = XxHash.XxHash32(span);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_CharSpan_ReturnsExpectedHash()
    {
        ReadOnlySpan<char> span = TestString.AsSpan();
        var hash = XxHash.XxHash32(span);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_ByteSpan_MatchesByteArray()
    {
        ReadOnlySpan<byte> span = TestBytes;
        var spanHash = XxHash.XxHash32(span);
        var arrayHash = XxHash.XxHash32(TestBytes);
        Assert.That(spanHash, Is.EqualTo(arrayHash));
    }

    [Test]
    public void XxHash32_CharSpan_MatchesString()
    {
        ReadOnlySpan<char> span = TestString.AsSpan();
        var spanHash = XxHash.XxHash32(span);
        var stringHash = XxHash.XxHash32(TestString);
        Assert.That(spanHash, Is.EqualTo(stringHash));
    }
#endif

    [Test]
    public void XxHash32_Stream_ReturnsExpectedHash()
    {
        using var stream = new MemoryStream(TestBytes);
        var hash = XxHash.XxHash32(stream);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_Stream_MatchesByteArray()
    {
        using var stream = new MemoryStream(TestBytes);
        var streamHash = XxHash.XxHash32(stream);
        var arrayHash = XxHash.XxHash32(TestBytes);
        Assert.That(streamHash, Is.EqualTo(arrayHash));
    }

    [Test]
    public void XxHash32_Stream_EmptyStream_ReturnsNonZero()
    {
        using var stream = new MemoryStream();
        var hash = XxHash.XxHash32(stream);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_Stream_Null_ThrowsException()
    {
        Assert.Throws<ArgumentNullException>(() => XxHash.XxHash32((Stream)null));
    }

    [Test]
    public void XxHash32_Stream_CustomBufferSize_ReturnsExpectedHash()
    {
        using var stream = new MemoryStream(TestBytes);
        var hash = XxHash.XxHash32(stream, 16);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_Stream_LargeData_ReturnsExpectedHash()
    {
        var largeData = new byte[10000];
        new Random(42).NextBytes(largeData);
        using var stream = new MemoryStream(largeData);
        var hash = XxHash.XxHash32(stream);
        Assert.That(hash, Is.Not.Zero);
    }

    #endregion XxHash32 Tests

    #region XxHash32s Tests (Signed)

    [Test]
    public void XxHash32s_String_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash32s(TestString);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32s_ByteArray_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash32s(TestBytes);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32s_MatchesUnsignedCast()
    {
        var unsignedHash = XxHash.XxHash32(TestString);
        var signedHash = XxHash.XxHash32s(TestString);
        Assert.That(signedHash, Is.EqualTo((int)unsignedHash));
    }

#if NETCOREAPP || NETSTANDARD2_1_OR_GREATER
    [Test]
    public void XxHash32s_ByteSpan_ReturnsExpectedHash()
    {
        ReadOnlySpan<byte> span = TestBytes;
        var hash = XxHash.XxHash32s(span);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32s_CharSpan_ReturnsExpectedHash()
    {
        ReadOnlySpan<char> span = TestString.AsSpan();
        var hash = XxHash.XxHash32s(span);
        Assert.That(hash, Is.Not.Zero);
    }
#endif

    [Test]
    public void XxHash32s_Stream_ReturnsExpectedHash()
    {
        using var stream = new MemoryStream(TestBytes);
        var hash = XxHash.XxHash32s(stream);
        Assert.That(hash, Is.Not.Zero);
    }

    #endregion XxHash32s Tests (Signed)

    #region Consistency Tests

    [Test]
    public void Hash_SameInput_ProducesSameOutput_ByteArray()
    {
        var hash1 = XxHash.XxHash64(TestBytes);
        var hash2 = XxHash.XxHash64(TestBytes);
        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void Hash_SameInput_ProducesSameOutput_String()
    {
        var hash1 = XxHash.XxHash32(TestString);
        var hash2 = XxHash.XxHash32(TestString);
        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void Hash_DifferentInputs_ProduceDifferentOutputs()
    {
        var hash1 = XxHash.XxHash64("test1");
        var hash2 = XxHash.XxHash64("test2");
        Assert.That(hash1, Is.Not.EqualTo(hash2));
    }

    [Test]
    public void Hash32_And_Hash64_ProduceDifferentResults()
    {
        var hash32 = XxHash.XxHash32(TestBytes);
        var hash64 = XxHash.XxHash64(TestBytes);
        Assert.That((ulong)hash32, Is.Not.EqualTo(hash64));
    }

    #endregion Consistency Tests

    #region Edge Cases

    [Test]
    public void XxHash64_SingleByte_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash64(new byte[] { 42 });
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_SingleByte_ReturnsExpectedHash()
    {
        var hash = XxHash.XxHash32(new byte[] { 42 });
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_AllZeroBytes_ReturnsNonZero()
    {
        var hash = XxHash.XxHash64(new byte[100]);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash32_AllZeroBytes_ReturnsNonZero()
    {
        var hash = XxHash.XxHash32(new byte[100]);
        Assert.That(hash, Is.Not.Zero);
    }

    [Test]
    public void XxHash64_16ByteBoundary_ReturnsExpectedHash()
    {
        var hash1 = XxHash.XxHash64(new byte[15]);
        var hash2 = XxHash.XxHash64(new byte[16]);
        var hash3 = XxHash.XxHash64(new byte[17]);
        Assert.That(hash1, Is.Not.EqualTo(hash2));
        Assert.That(hash2, Is.Not.EqualTo(hash3));
    }

    [Test]
    public void XxHash32_32ByteBoundary_ReturnsExpectedHash()
    {
        var hash1 = XxHash.XxHash32(new byte[31]);
        var hash2 = XxHash.XxHash32(new byte[32]);
        var hash3 = XxHash.XxHash32(new byte[33]);
        Assert.That(hash1, Is.Not.EqualTo(hash2));
        Assert.That(hash2, Is.Not.EqualTo(hash3));
    }

    #endregion Edge Cases

    #region Stream Edge Cases

    [Test]
    public void XxHash64_Stream_SmallBufferSize_MatchesLargeBuffer()
    {
        using var stream1 = new MemoryStream(TestBytes);
        using var stream2 = new MemoryStream(TestBytes);

        var hash1 = XxHash.XxHash64(stream1, 8);
        var hash2 = XxHash.XxHash64(stream2, 4096);

        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void XxHash32_Stream_SmallBufferSize_MatchesLargeBuffer()
    {
        using var stream1 = new MemoryStream(TestBytes);
        using var stream2 = new MemoryStream(TestBytes);

        var hash1 = XxHash.XxHash32(stream1, 8);
        var hash2 = XxHash.XxHash32(stream2, 4096);

        Assert.That(hash1, Is.EqualTo(hash2));
    }

    [Test]
    public void XxHash64_Stream_DataAcrossBufferBoundaries()
    {
        var data = new byte[100];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)(i % 256);
        }

        using var stream = new MemoryStream(data);
        var streamHash = XxHash.XxHash64(stream, 17);
        var arrayHash = XxHash.XxHash64(data);

        Assert.That(streamHash, Is.EqualTo(arrayHash));
    }

    [Test]
    public void XxHash32_Stream_DataAcrossBufferBoundaries()
    {
        var data = new byte[100];
        for (int i = 0; i < data.Length; i++)
        {
            data[i] = (byte)(i % 256);
        }

        using var stream = new MemoryStream(data);
        var streamHash = XxHash.XxHash32(stream, 17);
        var arrayHash = XxHash.XxHash32(data);

        Assert.That(streamHash, Is.EqualTo(arrayHash));
    }

    #endregion Stream Edge Cases

    #region Performance Verification Tests

    [Test]
    public void XxHash64_LargeData_CompletesInReasonableTime()
    {
        var largeData = new byte[1_000_000];
        new Random(42).NextBytes(largeData);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var hash = XxHash.XxHash64(largeData);
        stopwatch.Stop();

        Assert.That(hash, Is.Not.Zero);
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(1000), "Hash should complete within 1 second");
    }

    [Test]
    public void XxHash32_LargeData_CompletesInReasonableTime()
    {
        var largeData = new byte[1_000_000];
        new Random(42).NextBytes(largeData);

        var stopwatch = System.Diagnostics.Stopwatch.StartNew();
        var hash = XxHash.XxHash32(largeData);
        stopwatch.Stop();

        Assert.That(hash, Is.Not.Zero);
        Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThan(1000), "Hash should complete within 1 second");
    }

    #endregion Performance Verification Tests

    #region Known Value Tests

    [Test]
    public void XxHash64_EmptyInput_ReturnsKnownValue()
    {
        // xxHash64 of empty input with seed 0 is 0xEF46DB3751D8E999
        var hash = XxHash.XxHash64(Array.Empty<byte>());
        Assert.That(hash, Is.EqualTo(0xEF46DB3751D8E999UL));
    }

    [Test]
    public void XxHash32_EmptyInput_ReturnsKnownValue()
    {
        // xxHash32 of empty input with seed 0 is 0x02CC5D05
        var hash = XxHash.XxHash32(Array.Empty<byte>());
        Assert.That(hash, Is.EqualTo(0x02CC5D05U));
    }

    #endregion Known Value Tests
}
