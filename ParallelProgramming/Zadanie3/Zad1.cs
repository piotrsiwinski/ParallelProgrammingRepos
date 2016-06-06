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
        
        private Thread _producerThread;
        private Thread _consumerThread;
        private List<string> bufferList;
        private int _bufferSize;
        private string _password;
        private bool _passwordFound;

        public ProducerConsumer(int bufferSize, string password)
        {
            _producerAutoResetEvent = new AutoResetEvent(false);
            _consumerAutoResetEvent = new AutoResetEvent(false);
            _bufferSize = bufferSize;
            _password = password;
            _passwordFound = false;
            bufferList = new List<string>(_bufferSize);

            _producerThread = new Thread(Produce);
            _consumerThread = new Thread(Consume);

            _producerThread.Start();
            _consumerThread.Start();
            _producerThread.Join();

            
            _consumerThread.Join();


        }

        private void Produce()
        {
            while (!_passwordFound)
            {
                Console.WriteLine("Producing passwords");
                //Thread.Sleep(1000);
                while (bufferList.Count < _bufferSize)
                {
                    bufferList.Add(Membership.GeneratePassword(_password.Length, 0));
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
                foreach (var element in bufferList)
                {
                    if (element == _password)
                    {
                        _passwordFound = true;
                        Console.WriteLine($"Password found. Password is: {element}");
                        break;
                    }
                    
                }
                bufferList.Clear();
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
            ProducerConsumer producerConsumer = new ProducerConsumer(100, password);
            Console.ReadLine();
        }
    }
}
