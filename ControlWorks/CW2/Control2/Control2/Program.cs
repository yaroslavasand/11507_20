using System;

class Program
{
    static void Main()
    {
        MethodExecutor executor = new MethodExecutor();
        TestClass testObj = new TestClass { Count = 7 };
        
        int result1 = (int)executor.Execute(testObj, "Method0");
        Console.WriteLine($"Method0 result: {result1}");
        
        int result2 = (int)executor.Execute(testObj, "Method2");
        Console.WriteLine($"Method2 result (using Count=7): {result2}");
        
        QueueWorker queueWorker = new QueueWorker();
        int sum = queueWorker.ProcessQueue();
        Console.WriteLine($"Queue sum (0-99): {sum}");
        
        CombinedWorker combinedWorker = new CombinedWorker(testObj);
        int combinedSum = combinedWorker.ProcessMethodQueue();
        Console.WriteLine($"Combined sum: {combinedSum}");
    }
}