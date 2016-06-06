using System;
using System.Threading;

namespace Zadanie1
{
    public class Gnp
    {
        private readonly int[,] _graph;
        private readonly int _size;
        private readonly Random _random;
        public static int Egdes;
        private readonly object _locker = new object();

        public Gnp(int size)
        {
            _random = new Random();
            _graph = new int[size, size];
            _size = size;
        }

        public void GenerateGnpSync(double p)
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = i + 1; j < _size; j++)
                {
                    if (p * 100 >= _random.Next(0, 100))
                    {
                        _graph[i, j] = 1;
                        _graph[j, i] = 1;
                        continue;
                    }
                    _graph[i, j] = 0;
                    _graph[j, i] = 0;

                }
            }
        }

        public void GenerateGnpAsync(double p)
        {
            Thread[] threads = new Thread[_size];

            for (int i = 0; i < _size; i++)
            {
                var i1 = i;
                threads[i1] = new Thread(() =>
                {
                    GenerateGnpVertex(i1, p);
                });

                threads[i].Start();
                threads[i].Join();
            }
        }

        private void GenerateGnpVertex(int i, double p)
        {
            for (int j = i + 1; j < _size; j++)
            {
                if (p * 100 >= _random.Next(0, 100))
                {
                    _graph[i, j] = 1;
                    _graph[j, i] = 1;
                    continue;
                }
                _graph[i, j] = 0;
                _graph[j, i] = 0;
            }
        }

        public void ShowGraph()
        {
            for (int i = 0; i < _size; i++)
            {
                for (int j = 0; j < _size; j++)
                {
                    Console.Write(_graph[i, j]);
                }
                Console.WriteLine();
            }
        }

        public int CountEdgesSync()
        {
            int result = 0;
            for (int i = 0; i < _size; i++)
            {
                for (int j = i + 1; j < _size; j++)
                {
                    if (_graph[i, j] == 1)
                        result++;
                }

            }
            return result;
        }

        public int CountEgdesAsync()
        {
            Thread[] threads = new Thread[_size];

            for (int i = 0; i < _size; i++)
            {
                var i1 = i;
                threads[i1] = new Thread(() =>
                {
                    CountEdgesForVertex(i1);
                });

                threads[i].Start();
                threads[i].Join();
            }
            return Egdes;
        }

        public void CountEdgesForVertex(int vertexNumber)
        {
            int sum = 0;
            for (int i = vertexNumber; i < _size; i++)
            {
                if (_graph[vertexNumber, i] != 1) continue;
                sum++;
            }

            lock (_locker)
            {
                Egdes += sum;
            }
        }

    }

    public class Zad5
    {
        public static void Main(string[] args)
        {
            GraphGnp gnp = new GraphGnp(10);

            gnp.GenerateGnpSync(1);
            gnp.ShowGraph();
            Console.WriteLine();
            gnp.GenerateGnpAsync(1);
            gnp.ShowGraph();

            Console.ReadLine();
        }
    }
}