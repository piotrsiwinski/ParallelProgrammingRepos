using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie2._3
{
    public class ArrayAsync
    {
        private static readonly object _locker = new object();
        private readonly int[] _array;
        private readonly int _threadsCount;
        private readonly Thread[] _threads;
        private readonly Random _random;
        private int _sum;

       

        public ArrayAsync(int arraySize, int threadsCount = 4)
        {
            _sum = 0;
            _random = new Random();
            _array = new int[arraySize];
            _threadsCount = threadsCount;
            _threads = new Thread[threadsCount];

            for (int i = 0; i < arraySize; i++)
            {
                _array[i] = _random.Next(0, 100);
            }
        }

        public double SumArrayAsync()
        {
            for (int i = 0; i < _threadsCount; i++)
            {
                var temp = i;
                _threads[i] = new Thread(() =>
                {
                    SumArrayUsingThread(temp);
                });
                _threads[i].Start();
                _threads[i].Join();
            }

            return _sum;
        }

        private void SumArrayUsingThread(int index)
        {
            int result = 0;
            for (int i = index; i < _array.Length; i += _threadsCount)
            {
                result += _array[i];
            }
            Interlocked.Add(ref _sum, result);
        }

        public int SumArray()
        {
            return _array.Sum();
        }
    }

    class Zad1
    {
        static void Main(string[] args)
        {
            ArrayAsync arrayAsync = new ArrayAsync(100);
            Console.WriteLine($"Async sum {arrayAsync.SumArrayAsync()}");
            Console.WriteLine($"Sync sum {arrayAsync.SumArray()}");

            Console.ReadLine();
        }
    }
}
