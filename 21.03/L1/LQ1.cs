//LQ1. Используя Linq, циклически сдвинуть массив на K влево. Без циклов. Без Reverse.
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        int[] array = { 1, 2, 3, 4, 5 };
        int k = 2;
        
        int[] result = array.Skip(k).Concat(array.Take(k)).ToArray();
        
        Console.WriteLine(string.Join(" ", result));
    }
}