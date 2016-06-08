using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Zadanie2
{
    public class SearchElementInArrayAsync
    {
        private static readonly object _locker = new object();
        private readonly int[] _array;
        private readonly int _threadsCount;
        private readonly Thread[] _threads;
        private bool _isFound;
        private int _element;
        public double Sum { get; set; }

        public SearchElementInArrayAsync(int arraySize, int element, int threadsCount = 4)
        {
            var random = new Random();
            _array = new int[arraySize];
            _element = element;
            _threadsCount = threadsCount;
            _threads = new Thread[threadsCount];
            _isFound = false;

            for (int i = 0; i < arraySize; i++)
            {
                _array[i] = random.Next(1, 100);
            }
        }

        public bool FindElementAsync()
        {
            for (int i = 0; i < _threadsCount; i++)
            {
                var temp = i;
                _threads[i] = new Thread(() =>
                {
                    Console.WriteLine($"Utworzono watek od ID: {Thread.CurrentThread.ManagedThreadId}");
                    FindElementUsingThread(temp);
                });
                _threads[i].Start();
                _threads[i].Join();
            }

            return _isFound;
        }

        private void FindElementUsingThread(int temp)
        {
            for (int i = temp; i < _array.Length; i += _threadsCount)
            {
                if (_isFound)
                    break;
                if (_array[i] == _element)
                {
                    lock (_locker)
                    {
                        _isFound = true;
                    }
                    Console.WriteLine($"Element znaloziony przez watek {Thread.CurrentThread.ManagedThreadId}");
                }
                    

            }
        }

        class Zad5
        {
            static void Main(string[] args)
            {
                SearchElementInArrayAsync arrayAsync = new SearchElementInArrayAsync(100, 3);
                Console.WriteLine(arrayAsync.FindElementAsync() ? "Znaleziono element" : "Nie znaleziono elementu");

                Console.ReadLine();
            }
        }
    }
}
