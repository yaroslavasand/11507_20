using System;
using System.Diagnostics;
using System.IO;

class Program
{
    static void Main()
    {
        string path = "bigdata.txt";
        const int BUFFER_SIZE = 65536;
        const byte TARGET_BYTE = (byte)'A';
        
        if (!File.Exists(path))
        {
            using (var sw = new StreamWriter("bigdata.txt"))
            {
                for (int i = 0; i < 50_000_000; i++)
                    sw.WriteLine("Data line with some A symbols and other chars");
            }
        }
        
        var stopwatch = Stopwatch.StartNew();
        
        long totalCount = 0;
        byte[] buffer = new byte[BUFFER_SIZE];
        
        using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
        {
            int bytesRead;
            while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
            {
                for (int i = 0; i < bytesRead; i++)
                {
                    if (buffer[i] == TARGET_BYTE)
                        totalCount++;
                }
            }
        }
        
        stopwatch.Stop();
        
        Console.WriteLine($"Количество символов 'A': {totalCount}");
        Console.WriteLine($"Время выполнения: {stopwatch.ElapsedMilliseconds} мс");
    }
}