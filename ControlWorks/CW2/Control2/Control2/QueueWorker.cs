using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

public class QueueWorker
{
    private ConcurrentQueue<Func<int>> queue;
    private int totalSum;
    private object lockObject = new object();
    
    public int ProcessQueue()
    {
        queue = new ConcurrentQueue<Func<int>>();
        totalSum = 0;
        
        for (int i = 0; i < 100; i++)
        {
            int taskId = i;
            queue.Enqueue(() => taskId);
        }
        
        List<Thread> threads = new List<Thread>();
        
        for (int i = 0; i < 3; i++)
        {
            Thread thread = new Thread(WorkerMethod);
            thread.Start();
            threads.Add(thread);
        }
        
        foreach (Thread thread in threads)
        {
            thread.Join();
        }
        
        return totalSum;
    }
    
    private void WorkerMethod()
    {
        while (queue.TryDequeue(out Func<int> task))
        {
            int result = task();
            lock (lockObject)
            {
                totalSum += result;
            }
        }
    }
}