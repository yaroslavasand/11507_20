//LQ3. Дан набор строк. Найти все строки, в которых ни одна буква не повторяется больше 2 раз.
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        string[] strings = {
            "hello",
            "world",
            "aabbcc",
            "aaabbb",
            "abcabc",
            "aabbaa",
            "test",
            "unique",
            "aaaa"
        };

        Console.WriteLine("Исходный набор строк:");
        foreach (string s in strings)
        {
            Console.WriteLine($"  \"{s}\"");
        }
        
        var result = FindStringsWithMaxTwoRepeats(strings);

        Console.WriteLine("\nСтроки, где ни одна буква не повторяется больше 2 раз:");
        if (result.Length == 0)
        {
            Console.WriteLine("  (таких строк нет)");
        }
        else
        {
            foreach (string s in result)
            {
                Console.WriteLine($"  \"{s}\"");
            }
        }
    }

    static string[] FindStringsWithMaxTwoRepeats(string[] strings)
    {
        List<string> result = new List<string>();
        
        foreach (string s in strings)
        {
            if (IsValidString(s))
            {
                result.Add(s);
            }
        }

        return result.ToArray();
    }

    static bool IsValidString(string s)
    {
        Dictionary<char, int> counts = new Dictionary<char, int>();
        
        foreach (char c in s)
        {
            if (counts.ContainsKey(c))
                counts[c]++;
            else
                counts[c] = 1;
        }
        
        foreach (int count in counts.Values)
        {
            if (count > 2)
                return false;
        }

        return true;
    }
}