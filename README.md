
[![NuGet](https://img.shields.io/nuget/v/XxHash.svg)](https://www.nuget.org/packages/XxHash/) [![License](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE)

# XxHash.NET

A high-performance .NET implementation of the xxHash non-cryptographic hash algorithm.

## Features

- **Fast**: Optimized implementation using unsafe pointers for maximum performance
- **Complete**: Supports xxHash32, xxHash64, xxHash3 (64-bit), and xxHash128 algorithms
- **Flexible**: Multiple input types (string, byte array, Span, Stream)
- **Cross-platform**: Targets .NET Framework 4.5.2+ and .NET Standard 2.0+
- **Modern**: Full support for `Span<T>` on compatible frameworks
-  **Tested**: Comprehensive test suite with 170+ test cases

## Installation

Install via NuGet Package Manager:

```bash
dotnet add package System.IO.Hashing.XxHash
```

Or via Package Manager Console:

```powershell
Install-Package System.IO.Hashing.XxHash
```

## Supported Frameworks

- .NET Framework 4.5.2, 4.6.2, 4.7.2, 4.8, 4.8.1
- .NET Standard 2.0, 2.1
- .NET 5, 6, 7, 8, 9, 10

## Usage

### Basic Usage

```csharp
using System.IO.Hashing;

// Hash a string (64-bit)
ulong hash64 = XxHash.XxHash64("Hello, World!");

// Hash a byte array (32-bit)
byte[] data = new byte[] { 1, 2, 3, 4, 5 };
uint hash32 = XxHash.XxHash32(data);

// xxHash3 (newer, faster algorithm)
ulong hash3 = XxHash.XxHash3("Hello, World!");

// xxHash128 (128-bit hash)
var hash128 = XxHash.XxHash128("Hello, World!");
Console.WriteLine($"Hash128: {hash128}"); // Prints as hex string
```

### Working with Different Input Types

```csharp
// String
ulong hashFromString = XxHash.XxHash64("test");

// Byte array
byte[] bytes = { 0x48, 0x65, 0x6C, 0x6C, 0x6F };
ulong hashFromBytes = XxHash.XxHash64(bytes);

// ReadOnlySpan<byte> (.NET Standard 2.1+ / .NET Core 2.1+)
ReadOnlySpan<byte> span = stackalloc byte[] { 1, 2, 3, 4 };
ulong hashFromSpan = XxHash.XxHash64(span);

// ReadOnlySpan<char> (.NET Standard 2.1+ / .NET Core 2.1+)
ReadOnlySpan<char> charSpan = "Hello".AsSpan();
ulong hashFromCharSpan = XxHash.XxHash64(charSpan);

// Stream
using var stream = new FileStream("file.txt", FileMode.Open);
ulong hashFromStream = XxHash.XxHash64(stream);
```

### Choosing the Right Algorithm

```csharp
// xxHash32 - Fast, 32-bit output, good for hash tables
uint hash32 = XxHash.XxHash32(data);

// xxHash64 - Fast, 64-bit output, general purpose
ulong hash64 = XxHash.XxHash64(data);

// xxHash3 - Newest algorithm, optimized for modern CPUs, 64-bit output
ulong hash3 = XxHash.XxHash3(data);

// xxHash128 - 128-bit output, lowest collision rate
var hash128 = XxHash.XxHash128(data);
Console.WriteLine($"Low: {hash128.Low:X16}, High: {hash128.High:X16}");
byte[] hashBytes = hash128.ToByteArray(); // Convert to byte array
```

### Signed vs Unsigned Results

The library provides both signed and unsigned versions of the hash functions:

```csharp
// Unsigned (default)
ulong unsignedHash64 = XxHash.XxHash64("test");
uint unsignedHash32 = XxHash.XxHash32("test");
ulong unsignedHash3 = XxHash.XxHash3("test");

// Signed (convenience methods)
long signedHash64 = XxHash.XxHash64s("test");
int signedHash32 = XxHash.XxHash32s("test");
long signedHash3 = XxHash.XxHash3s("test");
```

### Custom Buffer Size for Streams

```csharp
using var stream = new FileStream("large-file.bin", FileMode.Open);

// Use a custom buffer size (default is 4096 bytes)
ulong hash = XxHash.XxHash64(stream, bufferSize: 8192);
```

## API Reference

### XxHash32 Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `XxHash32(string)` | `uint` | Hash a string |
| `XxHash32(byte[])` | `uint` | Hash a byte array |
| `XxHash32(ReadOnlySpan<byte>)` | `uint` | Hash a byte span* |
| `XxHash32(ReadOnlySpan<char>)` | `uint` | Hash a char span* |
| `XxHash32(Stream, int)` | `uint` | Hash a stream with optional buffer size |
| `XxHash32s(...)` | `int` | Signed versions of above methods |

### XxHash64 Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `XxHash64(string)` | `ulong` | Hash a string |
| `XxHash64(byte[])` | `ulong` | Hash a byte array |
| `XxHash64(ReadOnlySpan<byte>)` | `ulong` | Hash a byte span* |
| `XxHash64(ReadOnlySpan<char>)` | `ulong` | Hash a char span* |
| `XxHash64(Stream, int)` | `ulong` | Hash a stream with optional buffer size |
| `XxHash64s(...)` | `long` | Signed versions of above methods |

### XxHash3 Methods (Recommended)

| Method | Return Type | Description |
|--------|-------------|-------------|
| `XxHash3(string)` | `ulong` | Hash a string |
| `XxHash3(byte[])` | `ulong` | Hash a byte array |
| `XxHash3(ReadOnlySpan<byte>)` | `ulong` | Hash a byte span* |
| `XxHash3(ReadOnlySpan<char>)` | `ulong` | Hash a char span* |
| `XxHash3(Stream, int)` | `ulong` | Hash a stream with optional buffer size |
| `XxHash3s(...)` | `long` | Signed versions of above methods |

### XxHash128 Methods

| Method | Return Type | Description |
|--------|-------------|-------------|
| `XxHash128(string)` | `Hash128` | Hash a string |
| `XxHash128(byte[])` | `Hash128` | Hash a byte array |
| `XxHash128(ReadOnlySpan<byte>)` | `Hash128` | Hash a byte span* |
| `XxHash128(ReadOnlySpan<char>)` | `Hash128` | Hash a char span* |
| `XxHash128(Stream, int)` | `Hash128` | Hash a stream with optional buffer size |

\* *Available on .NET Standard 2.1+ and .NET Core 2.1+*

### Hash128 Structure

```csharp
public struct Hash128
{
    public ulong Low { get; }
    public ulong High { get; }
    
    public string ToString();        // Returns hex string representation
    public byte[] ToByteArray();     // Returns 16-byte array
}
```

## Algorithm Comparison

| Algorithm | Output Size | Speed | Use Case |
|-----------|-------------|-------|----------|
| **xxHash32** | 32-bit | Very Fast | Hash tables, simple checksums |
| **xxHash64** | 64-bit | Very Fast | General purpose, data deduplication |
| **xxHash3** | 64-bit | **Fastest** | Modern CPUs, recommended for new projects |
| **xxHash128** | 128-bit | Fast | Low collision probability, unique identifiers |

**Recommendation**: Use **xxHash3** for new projects as it's optimized for modern CPUs and provides the best performance-to-quality ratio.

## Performance

XxHash is designed for speed and provides excellent performance characteristics:

- Optimized unsafe pointer operations
- Zero-copy `Span<T>` support on modern frameworks
- Efficient streaming for large files
- Aggressive inlining of hot paths

### Benchmark Example

```csharp
var data = new byte[1_000_000];
new Random(42).NextBytes(data);

var stopwatch = Stopwatch.StartNew();
ulong hash = XxHash.XxHash3(data);
stopwatch.Stop();

Console.WriteLine($"Hashed 1MB in {stopwatch.ElapsedMilliseconds}ms");
// Typical result: < 1ms
```

## Known Hash Values

The implementation is validated against the reference xxHash implementation:

```csharp
// xxHash64 of empty input (seed 0)
Assert.Equal(0xEF46DB3751D8E999UL, XxHash.XxHash64(Array.Empty<byte>()));

// xxHash32 of empty input (seed 0)
Assert.Equal(0x02CC5D05U, XxHash.XxHash32(Array.Empty<byte>()));
```

## Error Handling

```csharp
// Null string or array returns 0
ulong hash = XxHash.XxHash64((string)null); // Returns 0

// Null stream throws ArgumentNullException
try
{
    XxHash.XxHash64((Stream)null);
}
catch (ArgumentNullException ex)
{
    Console.WriteLine("Stream cannot be null");
}

// Invalid buffer size throws ArgumentOutOfRangeException
try
{
    XxHash.XxHash64(stream, bufferSize: -1);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine("Buffer size must be positive");
}
```

## Use Cases

- **Hash tables**: Fast key generation for in-memory hash tables
- **Checksums**: Quick data integrity verification
- **Deduplication**: Efficient duplicate detection
- **Caching**: Generate cache keys from content
- **Distributed systems**: Consistent hashing for load balancing
- **Bloom filters**: Fast membership testing
- **Content-addressable storage**: Unique content identification

**Note**: xxHash is a non-cryptographic hash. Do NOT use it for:

- Password hashing
- Digital signatures
- Security-sensitive applications

For cryptographic purposes, use algorithms like SHA-256, SHA-3, or bcrypt.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Links

- [xxHash Official Repository](https://github.com/Cyan4973/xxHash)
- [xxHash Algorithm Specification](https://github.com/Cyan4973/xxHash/blob/dev/doc/xxhash_spec.md)
- [NuGet Package](https://www.nuget.org/packages/XxHash/)

