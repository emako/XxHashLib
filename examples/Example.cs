using System;
using System.IO.Hashing;

Console.WriteLine("XxHash.NET Example\n");

string testString = "Hello, World!";
byte[] testData = System.Text.Encoding.UTF8.GetBytes(testString);

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
Console.WriteLine($"XxHash128: {hash128}");
Console.WriteLine($"  Low:  {hash128.Low:X16}");
Console.WriteLine($"  High: {hash128.High:X16}");

byte[] hashBytes = hash128.ToByteArray();
Console.Write("  Bytes: ");
foreach (byte b in hashBytes)
{
    Console.Write($"{b:X2} ");
}
Console.WriteLine("\n");

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
