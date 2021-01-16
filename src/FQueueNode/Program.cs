using System;

namespace FQueueNode
{
    // Operacje:
    // - Peek - super szybka
    // - Enqueue(1 albo wiele)
    // - Dequeue(1 albo wiele)
    // - Count - super szybka
    // 
    // API:
    // GET /Dequeue? count = 1
    // GET /Count
    // GET /Peek
    // POST /Enqueue
    // 
    // 
    // 
    // Protokół:
    // Powtarzaj l razy:
    // 1B - tag length = n
    // nB - tag
    // 2B - msg length = k
    // kB - msg - read to end

    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
