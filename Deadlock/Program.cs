using System;
using System.Threading;

namespace Deadlock
{
    class Program
    {
        static object 
            mutex1 = new object(), 
            mutex2 = new object();
        static void Task1()
        {
            System.Console.WriteLine("Task1 started");
            lock (mutex1)
            {
                Thread.Sleep(100);
                lock (mutex2) {}
            }
        }

        static void Task2()
        {
            System.Console.WriteLine("Task2 started");
            lock (mutex2)
            {
                Thread.Sleep(50);
                lock (mutex1) {}
            }
        }

        static void Main(string[] args)
        {
            Thread t1 = new Thread(Task1);
            t1.Start();
            Thread t2 = new Thread(Task2);
            t2.Start();
            System.Console.WriteLine("END");
            Console.ReadLine();
        }
    }
}
