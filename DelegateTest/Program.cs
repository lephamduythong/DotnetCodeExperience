using System;

namespace DelegateTest
{
    delegate int MyDelegate(int input);

    class A
    {
        public int MyFunc(int input)
        {
            System.Console.WriteLine("1: " + input);
            return input;
        }

        public void MyVoid(MyDelegate callback)
        {
            System.Console.WriteLine("MyVoid does something");
            callback(1000);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var a = new A();

            // Get from another method
            MyDelegate md1 = a.MyFunc;
            md1.Invoke(100);
            System.Console.WriteLine("-----------------");

            // Init with "delegate" keyword
            MyDelegate md2 = delegate (int input) { System.Console.WriteLine("2: " + input); return input; };
            md2(69);
            System.Console.WriteLine("-----------------");

            // Init with lamda expression
            MyDelegate md3 = (int input) => { System.Console.WriteLine("3: " + input); return input; };
            md3(50);
            System.Console.WriteLine("-----------------");

            // Use "+=", "-=" operators
            md1 += md2;
            md1 += md3;
            md1 -= md3;
            md1(200);
            System.Console.WriteLine("-----------------");

            // Use as callback parameter
            a.MyVoid(md1);

            Console.ReadKey();
        }
    }
}
