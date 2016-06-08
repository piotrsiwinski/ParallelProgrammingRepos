using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Security;

namespace Zadanie3
{
    public class ProducerConsumer
    {
        private static AutoResetEvent _producerAutoResetEvent;
        private static AutoResetEvent _consumerAutoResetEvent;
        private static AutoResetEvent _consumerProducerAutoResetEvent;

        private List<string> _firstBuffer;
        private List<string> _secondBuffer;
        private readonly int _bufferSize;
        private readonly string _password;
        private bool _passwordFound;

        public ProducerConsumer(int bufferSize, string password)
        {
            _producerAutoResetEvent = new AutoResetEvent(false);
            _consumerAutoResetEvent = new AutoResetEvent(false);
            _consumerProducerAutoResetEvent = new AutoResetEvent(false);
            _bufferSize = bufferSize;
            _password = password;
            _passwordFound = false;
            _firstBuffer = new List<string>(_bufferSize);
            _secondBuffer = new List<string>(_bufferSize);

            var producerThread = new Thread(Produce);
            var consumerThread = new Thread(Consume);
            var producerConsumerThread = new Thread(ConsumeProduce);

            producerThread.Start();
            consumerThread.Start();
            producerConsumerThread.Start();

            producerThread.Join();
            consumerThread.Join();
            producerConsumerThread.Join();


        }

        

        private void Produce()
        {
            while (!_passwordFound)
            {
                Console.WriteLine("Producing passwords");
                //Thread.Sleep(1000);
                while (_firstBuffer.Count < _bufferSize)
                {
                    _firstBuffer.Add(Membership.GeneratePassword(_password.Length, 0));
                }
                Console.WriteLine("Produced passwords");
                _consumerAutoResetEvent.Set();
                _producerAutoResetEvent.WaitOne();
            }

        }

        private void Consume()
        {
            while (!_passwordFound)
            {
                Console.WriteLine("Consumer waiting for producer...");
                _consumerAutoResetEvent.WaitOne();
                Console.WriteLine("Receiving from buffer");
                //Thread.Sleep(1000);
                foreach (var element in _firstBuffer)
                {
                    if (element == _password)
                    {
                        _passwordFound = true;
                        Console.WriteLine($"Password found. Password is: {element} found by First Consumer");
                        break;
                    }
                    _secondBuffer.Add(element.ReverseString());
                }
                _firstBuffer.Clear();
                _consumerProducerAutoResetEvent.Set();
            }

        }

        private void ConsumeProduce()
        {
            while (!_passwordFound)
            {
                Console.WriteLine("Second consumer waiting for producer...");
                _consumerProducerAutoResetEvent.WaitOne();
                Console.WriteLine("Receiving from reversed buffer");
                //Thread.Sleep(1000);
                foreach (var element in _secondBuffer)
                {
                    if (element == _password)
                    {
                        _passwordFound = true;
                        Console.WriteLine($"Password found. Password is: {element} found by Second Consumer ");
                        
                        break;
                    }
                }
                _secondBuffer.Clear();
                _producerAutoResetEvent.Set();
            }
        }
    }
    class Zad1
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Type your password");
            var password = Console.ReadLine();
            ProducerConsumer producerConsumer = new ProducerConsumer(5, password);
            Console.ReadLine();
        }
    }

    public static class StringExtensions
    {
        public static string ReverseString(this string s)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = s.Length-1; i >= 0; i--)
            {
                builder.Append(s[i]);
            }
            return builder.ToString();
        }
    }
}

