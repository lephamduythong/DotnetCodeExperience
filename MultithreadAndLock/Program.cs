using System;
using System.Threading;

namespace MultithreadAndLock
{
    class Program
    {   
        public static object lockKey = new object();
        public static void Task1() 
        {
            lock (lockKey)
            {
                for (int i = 0; i < 1000; i++)
                {
                    System.Console.Write("1");
                }
            }
        }

        public static void Task2()
        {
            lock (lockKey)
            {
                for (int i = 0; i < 1000; i++)
                {
                    System.Console.Write("2");
                }
            }
        }
        
        public static void Main(string[] args)
        {
            Thread t1 = new Thread(Task1);
            t1.Start();
            Thread t2 = new Thread(Task2);
            t2.Start();
            Console.ReadLine();
        }
    }
}
