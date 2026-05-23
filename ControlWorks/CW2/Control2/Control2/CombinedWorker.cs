using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

public class CombinedWorker
{
    private ConcurrentQueue<string> methodQueue;
    private int totalSum;
    private object lockObject = new object();
    private object targetObject;
    
    public CombinedWorker(object target)
    {
        targetObject = target;
    }
    
    public int ProcessMethodQueue()
    {
        methodQueue = new ConcurrentQueue<string>();
        totalSum = 0;
        
        for (int i = 0; i < 100; i++)
        {
            methodQueue.Enqueue($"Method{i % 10}");
        }
        
        List<Thread> threads = new List<Thread>();
        
        for (int i = 0; i < 3; i++)
        {
            Thread thread = new Thread(CombinedWorkerMethod);
            thread.Start();
            threads.Add(thread);
        }
        
        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        
        return totalSum;
    }
    
    private void CombinedWorkerMethod()
    {
        MethodExecutor executor = new MethodExecutor();
        
        while (methodQueue.TryDequeue(out string methodName))
        {
            object result = executor.Execute(targetObject, methodName);
            int intResult = Convert.ToInt32(result);
            
            lock (lockObject)
            {
                totalSum += intResult;
            }
        }
    }
}