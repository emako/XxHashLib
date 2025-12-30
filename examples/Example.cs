using System.IO.Hashing;
using System.Text;

Console.WriteLine("XxHash.NET Example\n");

string testString = "Hello, World!";
byte[] testData = Encoding.UTF8.GetBytes(testString);

// XxHash32
Console.WriteLine("=== XxHash32 ===");
uint hash32 = XxHash.XxHash32(testString);
Console.WriteLine($"XxHash32: {hash32:X8}");
Console.WriteLine();

// XxHash64
Console.WriteLine("=== XxHash64 ===");
ulong hash64 = XxHash.XxHash64(testString);
Console.WriteLine($"XxHash64: {hash64:X16}");
Console.WriteLine();

// XxHash3 (NEW!)
Console.WriteLine("=== XxHash3 (64-bit) ===");
ulong hash3 = XxHash.XxHash3(testString);
Console.WriteLine($"XxHash3: {hash3:X16}");
Console.WriteLine();

// XxHash128 (NEW!)
Console.WriteLine("=== XxHash128 ===");
var hash128 = XxHash.XxHash128(testString);

// 1. ToString() - 32位十六进制字符串
Console.WriteLine("=== ToString() ===");
Console.WriteLine($"Hash: {hash128}");
Console.WriteLine();

// 2. 属性访问 (现在是只读的)
Console.WriteLine("=== Properties (Readonly) ===");
Console.WriteLine($"Low:  {hash128.Low:X16}");
Console.WriteLine($"High: {hash128.High:X16}");
Console.WriteLine();

// 3. 解构支持 (新增功能!)
Console.WriteLine("=== Deconstruction ===");
var (low, high) = hash128;
Console.WriteLine($"Deconstructed - Low: {low:X16}, High: {high:X16}");
Console.WriteLine();

// 4. 相等性比较 (优化后的实现)
Console.WriteLine("=== Equality Comparison ===");
var hash2 = XxHash.XxHash128(testString);
var hash3x = XxHash.XxHash128("Different Text");

Console.WriteLine($"hash == hash2: {hash128 == hash2}");
Console.WriteLine($"hash == hash3: {hash128 == hash3x}");
Console.WriteLine($"hash.Equals(hash2): {hash128.Equals(hash2)}");
Console.WriteLine();

// 5. ToByteArray() - 转换为字节数组
Console.WriteLine("=== ToByteArray() ===");
byte[] hashBytes = hash128.ToByteArray();
Console.Write("Bytes: ");
foreach (byte b in hashBytes)
{
    Console.Write($"{b:X2} ");
}
Console.WriteLine("\n");

// 6. GetHashCode() - 优化后的哈希码
Console.WriteLine("=== GetHashCode() ===");
Console.WriteLine($"HashCode: {hash128.GetHashCode()}");
Console.WriteLine();

// 7. 不可变性演示
Console.WriteLine("=== Immutability (Readonly Struct) ===");
Console.WriteLine("Hash128 is now a readonly struct:");
Console.WriteLine("  ? Properties are read-only");
Console.WriteLine("  ? Cannot be modified after creation");
Console.WriteLine("  ? Thread-safe by design");
Console.WriteLine();

// 8. 实际使用场景
Console.WriteLine("=== Practical Usage ===");
var fileHashes = new System.Collections.Generic.Dictionary<Hash128, string>();

// 使用 Hash128 作为字典键
fileHashes[XxHash.XxHash128("file1.txt")] = "File 1";
fileHashes[XxHash.XxHash128("file2.txt")] = "File 2";
fileHashes[XxHash.XxHash128("file3.txt")] = "File 3";

Console.WriteLine($"Dictionary with {fileHashes.Count} Hash128 keys created");

// 查找
var lookupHash = XxHash.XxHash128("file2.txt");
if (fileHashes.TryGetValue(lookupHash, out string value))
{
    Console.WriteLine($"Found: {value}");
}

Console.WriteLine();

// Performance comparison
Console.WriteLine("=== Performance Test (1MB data) ===");
byte[] largeData = new byte[1024 * 1024];
new Random(42).NextBytes(largeData);

var sw = System.Diagnostics.Stopwatch.StartNew();
for (int i = 0; i < 100; i++)
{
    XxHash.XxHash32(largeData);
}
sw.Stop();
Console.WriteLine($"XxHash32:  {sw.ElapsedMilliseconds}ms for 100 iterations");

sw.Restart();
for (int i = 0; i < 100; i++)
{
    XxHash.XxHash64(largeData);
}
sw.Stop();
Console.WriteLine($"XxHash64:  {sw.ElapsedMilliseconds}ms for 100 iterations");

sw.Restart();
for (int i = 0; i < 100; i++)
{
    XxHash.XxHash3(largeData);
}
sw.Stop();
Console.WriteLine($"XxHash3:   {sw.ElapsedMilliseconds}ms for 100 iterations");

sw.Restart();
for (int i = 0; i < 100; i++)
{
    XxHash.XxHash128(largeData);
}
sw.Stop();
Console.WriteLine($"XxHash128: {sw.ElapsedMilliseconds}ms for 100 iterations");

Console.WriteLine("\nPress any key to exit...");
Console.ReadKey();
