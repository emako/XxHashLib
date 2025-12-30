using System;
using System.IO.Hashing;

namespace Hash128Example
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hash128 Struct Optimization Demo\n");

            string text = "Hello, World!";
            var hash = XxHash.XxHash128(text);

            // 1. ToString() - 32位十六进制字符串
            Console.WriteLine("=== ToString() ===");
            Console.WriteLine($"Hash: {hash}");
            Console.WriteLine();

            // 2. 属性访问 (现在是只读的)
            Console.WriteLine("=== Properties (Readonly) ===");
            Console.WriteLine($"Low:  {hash.Low:X16}");
            Console.WriteLine($"High: {hash.High:X16}");
            Console.WriteLine();

            // 3. 解构支持 (新增功能!)
            Console.WriteLine("=== Deconstruction (NEW!) ===");
            var (low, high) = hash;
            Console.WriteLine($"Deconstructed - Low: {low:X16}, High: {high:X16}");
            Console.WriteLine();

            // 4. 相等性比较 (优化后的实现)
            Console.WriteLine("=== Equality Comparison ===");
            var hash2 = XxHash.XxHash128(text);
            var hash3 = XxHash.XxHash128("Different Text");
            
            Console.WriteLine($"hash == hash2: {hash == hash2}");  // true
            Console.WriteLine($"hash == hash3: {hash == hash3}");  // false
            Console.WriteLine($"hash.Equals(hash2): {hash.Equals(hash2)}");  // true
            Console.WriteLine();

            // 5. ToByteArray() - 转换为字节数组
            Console.WriteLine("=== ToByteArray() ===");
            byte[] bytes = hash.ToByteArray();
            Console.Write("Bytes: ");
            foreach (byte b in bytes)
            {
                Console.Write($"{b:X2} ");
            }
            Console.WriteLine("\n");

            // 6. GetHashCode() - 优化后的哈希码
            Console.WriteLine("=== GetHashCode() ===");
            Console.WriteLine($"HashCode: {hash.GetHashCode()}");
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

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
