using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zadanie4
{
    class Program
    {
        public static void ShowArray(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write(array[i]);
            }
            Console.WriteLine();
        }

        
        public static void Generate(int n, int[] array)
        {
            if (n == 1)
            {
                return;
            }
            else
            {
                for (int i = 0; i < array.Length; i++)
                {
                    Generate(n-1, array);
                    if (n%2 == 0)
                    {
                        var helper = array[i];
                        array[i] = array[n-1];
                        array[n-1] = helper;
                        
                    }
                    else
                    {
                        var helper = array[0];
                        array[0] = array[n - 1];
                        array[n - 1] = helper;
                        ShowArray(array);
                    }
                }
                ShowArray(array);
                Generate(n-1, array);
            }
        }

        
        static void Main(string[] args)
        {
            Program.Generate( 3, new[] {0, 1, 2});

            Console.Read();
        }
    }
}
