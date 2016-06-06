using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie2
{
    public class ArrayAsync
    {
        private static readonly object _locker = new object();
        private readonly double[] _array;
        private readonly int _threadsCount;
        private readonly Thread[] _threads;
        private readonly Random _random;
        public double Sum { get; set; }

        public ArrayAsync(int arraySize, int threadsCount = 4)
        {
            _random = new Random();
            _array = new double[arraySize];
            _threadsCount = threadsCount;
            _threads = new Thread[threadsCount];

            for (int i = 0; i < arraySize; i++)
            {
                _array[i] = _random.NextDouble();
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

            return Sum;
        }

        private void SumArrayUsingThread(int index)
        {
            double result = 0;
            for (int i = index; i < _array.Length; i += _threadsCount)
            {
                result += _array[i];
            }

            lock (_locker)
            {
                Sum += result;
            }
        }

        public double SumArray()
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
