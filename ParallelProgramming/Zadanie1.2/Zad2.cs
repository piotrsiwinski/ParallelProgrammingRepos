using System;
using System.Threading;

namespace ConsoleApplication1
{

    class Program
    {
       
        static int CreatedThreads = 0;
        static object lockMethod = new object();
        static void IncrementNumberOfThreads()
        {
            lock (lockMethod)
            {
                ++CreatedThreads;
            }
        }

        public static void CreateThread()
        {
            try
            {
                IncrementNumberOfThreads();

                Console.WriteLine($"Thread nr: {CreatedThreads} and id {Thread.CurrentThread.ManagedThreadId}");
                Thread thread = new Thread(CreateThread);
                thread.Start();
                thread.Join();
                Console.WriteLine($"Thread numer {Thread.CurrentThread.ManagedThreadId} is exiting");
            }
            catch (Exception)
            {
                Console.WriteLine($"Max number of threads: {CreatedThreads}");
            }
        }

        static void Main(string[] args)
        {
            try
            {
                Thread thr = new Thread(CreateThread);
                thr.Start();
            }
            catch (Exception)
            {
                Console.WriteLine("Exiting");
            }
        }
    }
}